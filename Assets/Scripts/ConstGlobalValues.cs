using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstGlobalValues : MonoBehaviour {
    public static ConstGlobalValues Instance;

    private void Awake() {
        Instance = this;
    }

    public int PLAYER_INVENTORY_SIZE;
    public float TIME_SPEED;
    public float TIME_SPEED_WHILE_NAPPING;
    public float NAPPING_DURATION;
    public int WAKE_UP_HOUR;
}

public static class Utils {
    public static T FindInArray<T>(T[] array, T item) {
        foreach (T t in array) {
            if (t != null && t.Equals(item))
                return t;
        }

        return default(T);
    }

    public static int FindIndexInArray<T>(T[] array, T item) {
        for (int i = 0; i < array.Length; i++) {
            if (array[i].Equals(item))
                return i;
        }

        return -1;
    }

    public static void AddInArray<T>(T[] array, T item) {

        for (int i = 0; i < array.Length; i++) {
            if (array[i] == null) {

                array[i] = item;
                return;
            }
        }
    }
    public static bool ArrayHasRoom<T>(T[] array) {
        foreach (var item in array) {
            if (item == null)
                return true;
        }

        return false;
    }
}