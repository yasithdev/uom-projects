#include <IRremote.h>
#include <IRremoteInt.h>
#include <EEPROM.h>


// -------------------------------
// DECLARE VARIABLES AND CONSTANTS
// -------------------------------

// CONSTANTS
const int IR_OUT = 9;
const int RECV_PIN = 4;

// VARIABLES
IRrecv ir_recv(RECV_PIN);
IRsend ir_send;
decode_results dr;
int mem_ptr;

// FLAGS
bool is_learn = false;
bool is_save = false;
bool is_back = false;
bool is_recorded = false;
// -------------------------------


// --------------------------
// RUN ONCE WHEN INITIALIZING
// --------------------------
void setup() {

  // Start IR_RECV Serial Communication
  ir_recv.enableIRIn();
  pinMode(IR_OUT, OUTPUT);

  // Start BT Serial Communication
  Serial1.begin(9600);
  Serial1.println("Bluetooth transmission active");

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
  if (Serial1.available() > 0) {

    // Read from buffer and sanitize it
    String input = Serial1.readString();
    input.toUpperCase(), input.trim();

    // Write received command to BT Serial
    Serial1.print("Command Received : " + input);

    // Execute received command
    switch (input[0]) {
      // LEARN
      case 'L': is_learn = true; is_recorded = false; break;
      // SAVE
      case 'S': is_save = true; break;
      // BACK
      case 'B': is_back = true; break;
      // RETRY
      case 'R': is_recorded = false; break;
      // TRANSMIT
      case 'T': executeTransmit(&dr); break;
    }
  }

  // ----------------------------------
  // Learning Mode
  // ----------------------------------
  if (is_learn) {

    // -------------------------------------
    // While Learning
    // -------------------------------------
    if (is_learn && !is_save && !is_back && !is_recorded) {

      // Read from IR Sensor and Write to BT Serial
      if (ir_recv.decode(&dr)) {

        // Save reading to EEPROM first
        EEPROM.write(mem_ptr, dr.value);

        // Update state as recorded
        is_recorded = true;

        // Writing Mem_Ptr and Reading to BT Serial
        Serial1.print(mem_ptr, HEX);
        Serial1.print(" | ");
        serialWrite(&dr);

        // Resume IR Read
        ir_recv.resume();
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
      is_recorded = false;
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
      is_recorded = false;
    }
  }
}
// ------------------


// ----------------------------------
// Transmit result to BT Serial
// ----------------------------------
void executeTransmit(decode_results* result) {

  // Write what will being transmitted to BT Serial
  Serial1.print("Transmitting >> "); serialWrite(result);

  // Transmit signal through IR
  // ir_send.sendRaw((unsigned int*) dr.rawbuf, dr.rawlen, 38);
  ir_send.sendNEC(result->value, 32);
}
// ----------------------------------


// ----------------------------------
// SerialWrite a decode_result rawbuf
// ----------------------------------
void serialWrite(decode_results* result) {
//  for (int i = 0; i < result->rawlen; i++) {
//    Serial1.print((unsigned int) result->rawbuf + i, HEX);
//  }
//  Serial1.print('\n');
    Serial1.println(result->value, HEX);
}
// ----------------------------------
