import serial
import time


def main():
    try:
        # Serielle Verbindungen initialisieren
        s_left = serial.Serial('COM8', 9600, timeout=1)
        s_right = serial.Serial('COM11', 9600, timeout=1)

        while True:
            try:
                # Daten von den seriellen Verbindungen lesen
                if s_left.in_waiting > 0:
                    data_left = s_left.readline().decode().strip()
                    if "shake" in data_left:
                        print("shakeleft")

                if s_right.in_waiting > 0:
                    data_right = s_right.readline().decode().strip()
                    if "shake" in data_right:
                        print("shakeright")
            except serial.SerialException as e:
                print(f"Serieller Fehler: {e}")

            time.sleep(0.1)  # Kleine Pause einfügen, um die CPU-Last zu verringern

    except Exception as e:
        print(f"Ein Fehler ist aufgetreten: {e}")
    finally:
        # Ressourcen freigeben
        s_left.close()
        s_right.close()


main()
