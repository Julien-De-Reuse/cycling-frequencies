const int buttonPlus = 2;   // + or confirm
const int buttonMinus = 3;  // - or cancel
const int sensorPin = 12;

unsigned long pressStartTime = 0;
unsigned long lastPressTime = 0;

const unsigned long holdTimeSingle = 80;    // 
const unsigned long holdTimeConfirm = 800; // 1000ms for both buttons
const unsigned long debounceDelay = 1000;

// Hall Sensor
unsigned long lastTime = 0;
unsigned long lastSignalTime = 0;
float wheelCircumference = 128.0; // cm
unsigned long timeoutDuration = 750;
float displayedSpeed = 0.0;
float targetSpeed = 0.0;
unsigned long lastUpdateTime = 0;
unsigned long smoothingInterval = 500;
int lastSensorStatus = HIGH;

void setup() {
  pinMode(buttonPlus, INPUT_PULLUP);
  pinMode(buttonMinus, INPUT_PULLUP);
  pinMode(sensorPin, INPUT);
  Serial.begin(9600);
}

void loop() {
  unsigned long now = millis();
  handleHallSensor(now);
  handleButtons(now);
}

void handleButtons(unsigned long now) {
  bool plusPressed = !digitalRead(buttonPlus);
  bool minusPressed = !digitalRead(buttonMinus);

  if (plusPressed && !minusPressed) {
    if (pressStartTime == 0) pressStartTime = now;
    if (now - pressStartTime >= holdTimeSingle && now - lastPressTime > debounceDelay) {
      Serial.println("+");
      lastPressTime = now;
      pressStartTime = 0;
    }
  } else if (minusPressed && !plusPressed) {
    if (pressStartTime == 0) pressStartTime = now;
    if (now - pressStartTime >= holdTimeSingle && now - lastPressTime > debounceDelay) {
      Serial.println("-");
      lastPressTime = now;
      pressStartTime = 0;
    }
  } else if (plusPressed && minusPressed) {
    if (pressStartTime == 0) pressStartTime = now;
    if (now - pressStartTime >= holdTimeConfirm && now - lastPressTime > debounceDelay) {
      Serial.println("C");
      lastPressTime = now;
      pressStartTime = 0;
    }
  } else {
    pressStartTime = 0;
  }
}

void handleHallSensor(unsigned long currentTime) {
  int sensorStatus = digitalRead(sensorPin);

  if (lastSensorStatus == HIGH && sensorStatus == LOW) {
    lastSignalTime = currentTime;

    if (lastTime > 0) {
      unsigned long timeDiff = currentTime - lastTime;
      float timeInSeconds = timeDiff / 1000.0;
      float speedCmPerSec = wheelCircumference / timeInSeconds;
      float speedKph = (speedCmPerSec * 3600) / 100000;
      targetSpeed = speedKph;
    }
    lastTime = currentTime;
  }

  lastSensorStatus = sensorStatus;

  if (currentTime - lastSignalTime > timeoutDuration) {
    targetSpeed = 0.0;
  }

  if (currentTime - lastUpdateTime >= smoothingInterval) {
    lastUpdateTime = currentTime;
    displayedSpeed = (displayedSpeed + targetSpeed) / 2.0;
    Serial.println(displayedSpeed);
  }
}
