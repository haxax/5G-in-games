import websocket
import threading
import time
import json

def on_message(ws, message):
    print("Received message:", message)
    # You can parse the JSON message here if needed
    # data = json.loads(message)
    # Process the data as required

def on_error(ws, error):
    print("WebSocket error:", error)

def on_close(ws, close_status_code, close_msg):
    print("WebSocket connection closed")

def on_open(ws):
    print("Connected to the server")
    # If the server expects any initial messages, send them here
    # ws.send(json.dumps({'command': 'start'}))

if __name__ == "__main__":
    websocket.enableTrace(False)  # Set to True for detailed logs
    ws_url = "ws://127.0.0.1:5000/ws"

    ws = websocket.WebSocketApp(
        ws_url,
        on_open=on_open,
        on_message=on_message,
        on_error=on_error,
        on_close=on_close
    )

    wst = threading.Thread(target=ws.run_forever)
    wst.daemon = True
    wst.start()

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        print("Closing connection...")
        ws.close()
