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
    //Right
    if(inData.indexOf("c1") > 0)
    {
      if(inData.charAt(4) == '0')
      {        
        digitalWrite(left, LOW);
      }
      else
      {
        digitalWrite(left, HIGH);
      }
    }

    //Buzzer
    if(inData.indexOf("c2") > 0)
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

    //Right
    if(inData.indexOf("c3") > 0)
    {
      if(inData.charAt(4) == '0')
      {        
        digitalWrite(right, LOW);
      }
      else
      {
        digitalWrite(right, HIGH);
      }
    }
  }

  inData = "";  
}
