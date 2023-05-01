const SOCKET_URL = "ws://localhost:5000";
class WebSocketService {
    constructor() {
        this.socket = new WebSocket(SOCKET_URL);
        this.listeners = [];
        this.init();
    }

    init() {
        this.socket.addEventListener("open", () => {
            console.log("Connected to WebSocket server");
        });

        this.socket.addEventListener("close", () => {
            console.log("Disconnected from WebSocket server");
        });

        this.socket.addEventListener("message", (event) => {
            console.log("Received message from WebSocket server: ", event.data);
            this.listeners.forEach((listener) => {
                listener(event.data);
            });
        });
    }

    sendMessage(action, data) {
        console.log(`Sending message to WebSocket server: { action: ${action}, data: ${data}}`);
        this.socket.send(JSON.stringify({ action, data }));
    }

    addListener(listener) {
        this.listeners.push(listener);
    }

    removeListener(listener) {
        this.listeners = this.listeners.filter((l) => l !== listener);
    }
}

const webSocketServiceInstance = new WebSocketService();
export default webSocketServiceInstance;
