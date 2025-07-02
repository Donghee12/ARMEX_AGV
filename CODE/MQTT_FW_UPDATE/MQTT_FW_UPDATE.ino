#include <ESP8266WiFi.h>      // 📶 WiFi support for ESP8266
#include <PubSubClient.h>     // 📡 MQTT client library

// ===== Wi-Fi and MQTT Broker Configuration =====
const char* ssid = "TP-Link_BEF7";             // Wi-Fi SSID
const char* password = "69909177";             // Wi-Fi password
const char* mqtt_server = "192.168.0.101";     // MQTT broker IP (e.g., your PC)

WiFiClient espClient;              // TCP client for MQTT
PubSubClient client(espClient);    // MQTT client object

// ===== Connect to Wi-Fi =====
void setup_wifi() {
  WiFi.begin(ssid, password);      // Start Wi-Fi connection
  while (WiFi.status() != WL_CONNECTED) delay(500);  // Wait until connected

}

// ===== MQTT Callback =====
// This function is automatically called by the PubSubClient library
// whenever a subscribed MQTT message is received.
void callback(char* topic, byte* payload, unsigned int length) {
  String msg;
  for (int i = 0; i < length; i++) msg += (char)payload[i];  // Convert bytes to string

  // Check if the topic is for the Arduino (from PC or Pi)
  if (String(topic) == "PC_To/Arduino" || String(topic) == "Pi_To/Arduino") {
    Serial.println(msg);  // Send the message to Arduino Uno via serial
  }
}

// ===== Reconnect to MQTT Broker if Disconnected =====
void reconnect() {
  while (!client.connected()) {
    if (client.connect("ESPSerialRelay")) {       // Connect with client ID
      client.subscribe("PC_To/Arduino");          // Subscribe to PC → Arduino messages
      client.subscribe("Pi_To/Arduino");          // Subscribe to Pi → Arduino messages
    } else {
      delay(5000);  // Wait and retry
    }
  }
}

// ===== Setup Function (runs once on boot) =====
void setup() {
  Serial.begin(9600);                    // UART to Arduino Uno
  setup_wifi();                          // Connect to Wi-Fi
  client.setServer(mqtt_server, 1883);   // Set MQTT broker address
  client.setCallback(callback);          // Register the callback function
}

// ===== Main Loop =====
void loop() {
  // 🔄 Reconnect to MQTT broker if needed
  if (!client.connected()) reconnect();

  // ⚙️ Process incoming MQTT messages (calls callback() when needed)
  client.loop();

  // 📤 Read from Arduino Uno (via serial) and publish to MQTT
  if (Serial.available()) {
    String msg = Serial.readStringUntil('\n');  // Read serial input
    msg.trim();                                 // Remove whitespace

    if (msg.length() > 0) {
      clinet.publish("Arduino_To/Robot", msg.c_str());
      client.publish("Arduino_To/Pi", msg.c_str());  // Send to Pi
      client.publish("Arduino_To/PC", msg.c_str());  // Send to PC
    }
  }
}
