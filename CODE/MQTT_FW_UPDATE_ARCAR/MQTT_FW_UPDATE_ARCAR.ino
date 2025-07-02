#include <ESP8266WiFi.h>
#include <PubSubClient.h>

// ===== Wi-Fi and MQTT Configuration =====
const char* ssid = "TP-Link_BEF7";
const char* password = "69909177";
const char* mqtt_server = "192.168.0.101";

// Static IP config (use a different IP for ESP1)
IPAddress local_IP(192, 168, 0, 111);   // 🔧 <- Change as needed
IPAddress gateway(192, 168, 0, 1);
IPAddress subnet(255, 255, 255, 0);
IPAddress dns(8, 8, 8, 8);

WiFiClient espClient;
PubSubClient client(espClient);

// ===== Connect to Wi-Fi =====
void setup_wifi() {
  WiFi.config(local_IP, gateway, subnet, dns);  // ✅ Static IP
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\n✅ Wi-Fi Connected!");
  Serial.print("📡 IP Address: ");
  Serial.println(WiFi.localIP());
}

// ===== MQTT Callback =====
void callback(char* topic, byte* payload, unsigned int length) {
  String msg;
  for (unsigned int i = 0; i < length; i++) msg += (char)payload[i];
  msg.trim();

  if (String(topic) == "PC_To/ArCAR" || String(topic) == "Pi_To/ArCAR") {
    Serial.println(msg);   // Relay to Arduino
    Serial.flush();
  }
}

// ===== MQTT Reconnect =====
void reconnect() {
  while (!client.connected()) {
    Serial.println("Trying MQTT reconnect...");
    if (client.connect("ESP_ArCAR2")) {  // ✅ Unique client ID
      client.subscribe("PC_To/ArCAR");
      client.subscribe("Pi_To/ArCAR");
      Serial.println("MQTT Connected!");
    } else {
      Serial.print("Failed, rc=");
      Serial.print(client.state());
      Serial.println(" → retry in 5 sec");
      delay(5000);
    }
  }
}

// ===== Setup =====
void setup() {
  Serial.begin(9600);
  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
}

// ===== Main Loop =====
void loop() {
  if (WiFi.status() != WL_CONNECTED) setup_wifi();
  if (!client.connected()) reconnect();
  client.loop();

  if (Serial.available()) {
    String msg = Serial.readStringUntil('\n');
    msg.trim();
    if (msg.length() > 0) {
      client.publish("ArCAR_To/PC", msg.c_str());
      client.publish("ArCAR_To/Pi", msg.c_str());
    }
  }
}