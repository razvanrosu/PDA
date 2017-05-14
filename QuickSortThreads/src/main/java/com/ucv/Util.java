package com.ucv;

/**
 * Created by Razvan on 5/13/2017.
 */
public class Util {

    public static void swap(int[] array, int i, int j) {
        int tmp = array[i];
        array[i] = array[j];
        array[j] = tmp;
    }

    public static int median(int a, int b, int c) {
        if (a <= b) {
            if (c <= a) {
                return a;
            }

            return b <= c ? b : c;
        }

        if (c <= b) {
            return b;
        }

        return a <= c ? a : c;
    }
}
