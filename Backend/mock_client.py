import socketio

# Create a Socket.IO client
sio = socketio.Client()

# Event: Connected to the server
@sio.event
def connect():
    print("Connected to the server")
    # Request location updates
    sio.emit("start_location_updates")

# Event: Received location update
@sio.on("location_update")
def on_location_update(data):
    print("Received location update:", data)

# Event: Error from the server
@sio.on("error")
def on_error(data):
    print("Error from server:", data)

# Event: Disconnected from the server
@sio.event
def disconnect():
    print("Disconnected from server")

# Connect to the Socket.IO server
sio.connect("http://127.0.0.1:5000")

# Wait for events
sio.wait()
