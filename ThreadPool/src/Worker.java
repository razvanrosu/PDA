import java.util.Queue;

public class Worker extends Thread {

	private Queue<Task> tasks;
	private int[][] arr1;
	private int[][] arr2;
	private int[][] result;
	private int i;
	private int j;
	private int arr2Line;
	private int resultColumn;

	public Worker(Queue<Task> tasks, int[][] arr1, int[][] arr2, int arr2Line, int resultColumn, int[][] result) {
		this.tasks = tasks;
		this.arr1 = arr1;
		this.arr2 = arr2;
		this.arr2Line = arr2Line;
		this.resultColumn = resultColumn;
		this.result = result;

	}

	public void run() {
		int line;
		int sum = 0;
		Task task;
		while (!tasks.isEmpty()) {
			synchronized (tasks) {
				task = tasks.remove();
			}
			line = task.getLine();
			for (j = 0; j < resultColumn; j++) {
				sum = 0;
				for (i = 0; i < arr2Line; i++) {
					sum = sum + (arr1[line][i] * arr2[i][j]);
				}
				result[line][j] = sum;
			}
		}

	}
}