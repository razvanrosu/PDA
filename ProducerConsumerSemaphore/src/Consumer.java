import java.util.concurrent.Semaphore;

class Consumer implements Runnable{
 
    Semaphore semaphoreConsumer;
    Semaphore semaphoreProducer;
    
    public Consumer(Semaphore semaphoreConsumer,Semaphore semaphoreProducer) {
           this.semaphoreConsumer=semaphoreConsumer;
           this.semaphoreProducer=semaphoreProducer;
    }
 
    public void run() {
           
           for(int i=1;i<=7;i++){
                  try {
                      semaphoreConsumer.acquire(1);
                      System.out.println("Consumed : "+i);
                      semaphoreProducer.release();
                  } catch (InterruptedException e) {
                        e.printStackTrace();
                  }
           }
    }
}
    