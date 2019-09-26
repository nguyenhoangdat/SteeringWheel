#include <SoftwareSerial.h>
#include <Servo.h>
int right = 13, buzzer = 12, left = 11;
String inData = "";
void setup()
{
  pinMode(right, OUTPUT);
  pinMode(buzzer, OUTPUT);
  pinMode(left, OUTPUT);
  Serial.begin(9600);
}

void loop()
{
  while (Serial.available())
  { 
    delay(10); 
    char c = Serial.read(); 
    inData += c; 
  }

  if(inData.length() == 6 && 
     inData.indexOf("#") == 0 && 
     inData.indexOf("$") == 1 && 
     inData.indexOf("%") == 5)
  {
    //Direction
    if(inData.indexOf("cd") > 0)
    {
      if(inData.charAt(4) == '0')
      {        
        digitalWrite(left, HIGH);
        digitalWrite(right, LOW);
      }
      else
      {
        digitalWrite(left, LOW);
        digitalWrite(right, HIGH);
      }
    }

    //Buzzer
    if(inData.indexOf("cb") > 0)
    {
      if(inData.charAt(4) == '0')
      {        
        digitalWrite(buzzer, LOW);
      }
      else
      {
        digitalWrite(buzzer, HIGH);
      }
    }
  }

  inData = "";  
}
