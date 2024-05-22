//https://circuitdigest.com/microcontroller-projects/interfacing-flex-sensor-with-arduino
//https://www.instructables.com/How-to-use-a-Flex-Sensor-Arduino-Tutorial/
#include <Adafruit_CircuitPlayground.h>

float VCC = 3.3;
float R2 = 10000; // 10K resistor is
float sensorMinResistance = 200; // Value of the Sensor when its flat
float sensorMaxResistance = 4000; // Value of the Sensor when its bent at 90*

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  CircuitPlayground.begin();
  pinMode(A1, INPUT);
  pinMode(A2, INPUT);
  
}

void loop() {
  // put your main code here, to run repeatedly:
  Serial.flush();
  
  int ADCRaw = analogRead(A1);
  float ADCVoltage = (ADCRaw * VCC) / 1023; // get the voltage e.g (512 * 5) / 1023 = 2.5V
  float Resistance = R2 * (VCC / ADCVoltage - 1);
  float ReadValue = map(Resistance, sensorMinResistance, sensorMaxResistance, 0, 255);

  int ACDRaw1 = analogRead(A2);
  float ADCVoltage1 = (ACDRaw1 * VCC) / 1023;
  float Resistance1 = R2 * (VCC / ADCVoltage1 -1);
  float ReadValue1 = map(Resistance1, sensorMinResistance, sensorMaxResistance, 0, 255);

  int touchValue = CircuitPlayground.readCap(A3);

  Serial.print(ReadValue);

  Serial.print(" ");

  Serial.print(ReadValue1);

  Serial.print(" ");

  Serial.println(touchValue);

  delay(100);
}