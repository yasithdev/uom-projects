#include <EEPROM.h>
#include <IRremote.h>

// -------------------------------
// DECLARE VARIABLES AND CONSTANTS
// -------------------------------

// CONSTANTS
const int IR_OUT = 13;
const int RECV_PIN = 3;

// VARIABLES
IRrecv IR(RECV_PIN);
IRsend IRS
decode_results dr;
int mem_ptr;

// FLAGS
bool is_learn = false;
bool is_save = false;
bool is_back = false;
// -------------------------------


// --------------------------
// RUN ONCE WHEN INITIALIZING
// --------------------------
void setup() {

  // Start IR_RECV Serial Communication
  Serial.begin(9600);
  IR.enableIRIn();

  // Start BT Serial Communication
  Serial1.begin(9600);
  Serial1.println("Bluetooth transmission active");

  // Configure IR_EMIT PIN
  pinMode(IR_OUT, OUTPUT);

  // Clear the last bit of EEPROM (to initialize mem_ptr) [Dev Only]
  EEPROM.write(EEPROM.length() - 1, 0);

  // Memory pointer is at last byte of EEPROM
  mem_ptr = EEPROM.read(EEPROM.length() - 1);
}
// --------------------------


// ------------------
// EXECUTE IN A LOOP
// ------------------
void loop() {

  // ----------------------------------
  // Listening to BT Serial
  // ----------------------------------
  if (Serial1.available()) {

    // Read from buffer
    String input = Serial1.readString();
    input.trim();

    // Handle inputs
    if (input == "LEARN") {
      Serial1.println("LEARN command received");
      if (!is_learn && !is_save && !is_back) {
        is_learn = true;
      }
    }
    if (input == "SAVE") {
      Serial1.println("SAVE command received");
      if (is_learn && !is_back && !is_save) {
        is_save = true;
      }
    }
    if (input == "BACK") {
      Serial1.println("BACK command received");
      if (is_learn && !is_back && !is_save) {
        is_back = true;
      }
    }
    if (input.startsWith("TRANSMIT")) {
      int address = input.substring(9).toInt();
      int value = EEPROM.read(address);
      Serial1.print("Trasmitting >> ");
      Serial1.println(value, HEX);
      if (!is_learn && !is_save && !is_back) {
        Serial2.print(value);
      }
    }
  }

  // ----------------------------------
  // Learning Mode
  // ----------------------------------
  if (is_learn) {

    // -------------------------------------
    // While Learning
    // -------------------------------------
    if (is_learn && !is_save && !is_back) {

      // Read from IR Sensor and Write to BT Serial
      if (IR.decode(&dr)) {

        // Execute only for transients (Not press-and-hold)
        if (dr.value != 0xFFFFFFFF) {

          // Save reading to EEPROM first
          EEPROM.write(mem_ptr, dr.value);

          // Writing Mem_Ptr and Reading to BT Serial
          Serial1.print(mem_ptr, HEX);
          Serial1.print("|");
          Serial1.println(dr.value, HEX);
        }

        // Resume IR Read
        IR.resume();
      }
    }

    // -------------------------------------
    // If SAVE command issued
    // -------------------------------------
    if (is_save) {

      // Increase mem_ptr by 1
      mem_ptr++;

      // Update EEPROM with new mem_ptr value
      EEPROM.write(EEPROM.length() - 1, mem_ptr);

      // Clear flags
      is_learn = false;
      is_save = false;
      is_back = false;
    }

    // -------------------------------------
    // If BACK command issued
    // -------------------------------------
    if (is_back) {

      // Update EEPROM at mem_ptr with 0
      EEPROM.write(mem_ptr, 0);

      // Clear flags
      is_learn = false;
      is_save = false;
      is_back = false;
    }
  }
}
// ------------------
