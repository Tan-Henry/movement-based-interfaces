//shake-tutorial: https://www.youtube.com/watch?v=5gkT5oLRc2c
#include <Adafruit_CircuitPlayground.h>

float filteredShake = 0;
boolean isShaking = false;

void setup() {
  // put your setup code here, to run once:
  CircuitPlayground.begin();
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  Serial.flush();
  
  // Get accelerometer readings: values are between -10 and 10, 0 is neutral position
  float x = CircuitPlayground.motionX();
  float y = CircuitPlayground.motionY();
  float z = CircuitPlayground.motionZ();
  
  float shake = sqrt(x*x + y*y + z*z);
  shake -= 9.81;
  shake *= shake;
  filteredShake = (shake * 0.1) + (filteredShake * (1 - 0.1));

  if (filteredShake > 15) {
    if (!isShaking){
      isShaking = true;
      Serial.println("shake");
    }
  } else {
    isShaking = false;
  }

  delay(100);
}
