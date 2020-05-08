import java.util.concurrent.Semaphore;

public class Main {

	// Number of slices per pizza
	private static final int PIZZA_SLICES = 8;
	// Number of students in study session
	private static final int NUM_STUDENTS = 100;

	// Number of pizza slices at a given instance
	private static int slices = 0;
	// Semaphore to ensure mutual exclusion when accessing slices
	private static Semaphore mutex = new Semaphore(1);
	// Semaphore to ensure delivery guy waits until notified
	private static Semaphore order = new Semaphore(0);
	// Semaphore to ensure students wait until delivery guys delivers pizza
	private static Semaphore delivered = new Semaphore(0);

	public static void main(String[] args) {

		// Student thread
		class Student extends Thread {
			@Override
			public void run() {
				while (true) {
					try {
						// Acquire mutex lock
						mutex.acquire();
						if (slices == 0) {
							System.out.println("Out Of Pizza. Notifying...");
							// Signal pizza guy to deliver pizza
							order.release();
							// Wait until pizza guy notifies of pizza delivery
							delivered.acquire();
						}
						// Take one slice
						slices -= 1;
						// Release mutex lock after taking one slice
						mutex.release();
						// Study while eating the slice
						System.out.println("Studying while eating");
						Thread.sleep(1000);
					}
					catch (InterruptedException ex) {
						System.out.println("InterruptedException in Student.run");
					}
				}
			}
		}

		// Delivery guy thread
		class DeliveryGuy extends Thread {
			@Override
			public void run() {
				while (true) {
					try {
						// Wait until order is arrived
						order.acquire();
						// Deliver pizza
						Thread.sleep(5000);
						slices += PIZZA_SLICES;
						System.out.println("Delivered Pizza");
						// Notify students that pizza is delivered
						delivered.release();
					}
					catch (InterruptedException ex) {
						System.out.println("InterruptedException in DeliveryGuy.run");
					}
				}
			}
		}

		// Start multiple student threads
		for (int i = 0; i < NUM_STUDENTS; i++) {
			new Student().start();
		}
		// Start one delivery thread for Kamal's Pizza
		new DeliveryGuy().start();
	}
}
