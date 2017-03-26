import java.io.FileNotFoundException;
import java.io.FileReader;
import java.util.LinkedList;
import java.util.Queue;
import java.util.Scanner;

public class App {

	public static void main(String[] args) throws FileNotFoundException, InterruptedException {

		Queue<Task> tasks = new LinkedList<Task>();

		int[][] array1;
		int[][] array2;

		Scanner in = new Scanner(new FileReader("src/arrays.txt"));
		int m;
		int n;
		int resultLine;
		int resultColumn;
		int[][] result;
		int i;
		int j;

		m = in.nextInt();
		n = in.nextInt();
		resultLine = m;
		array1 = new int[m][n];
		for (i = 0; i < m; i++) {

			for (j = 0; j < n; j++) {

				array1[i][j] = in.nextInt();
			}

		}

		m = in.nextInt();
		n = in.nextInt();

		resultColumn = n;
		array2 = new int[m][n];

		for (i = 0; i < m; i++) {

			for (j = 0; j < n; j++) {

				array2[i][j] = in.nextInt();
			}

		}

		in.close();

		result = new int[resultLine][resultColumn];

		for (i = 0; i < resultLine; i++) {
			tasks.add(new Task(i));
		}

		for (i = 0; i < resultLine / 2; i++) {
			new Worker(tasks, array1, array2, m, resultColumn, result).start();
		}

		Thread.sleep(1000);

		for (i = 0; i < resultLine; i++) {
			for (j = 0; j < resultColumn; j++) {
				System.out.print(result[i][j] + " ");
			}
			System.out.println("\n");
		}

	}

}
