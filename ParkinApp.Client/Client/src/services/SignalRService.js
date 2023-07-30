import * as signalR from "@microsoft/signalr";

export default class SignalRService {
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5169/parkingSpotHub")
            .withAutomaticReconnect()
            .build();
    }

    startConnection() {
        this.connection.start()
            .then(() => console.log("Connection started"))
            .catch(err => console.log("Error while starting connection: " + err));

        return this.connection;
    }

    stopConnection() {
        this.connection.stop();
    }

    getConnection() {
        return this.connection;
    }
}
