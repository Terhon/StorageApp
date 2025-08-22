import {useEffect, useState} from "react";
import * as signalR from "@microsoft/signalr";


export function ChatFeature() {
    const [connection, setConnection] = useState(null);
    const [inputText, setInputText] = useState("");
    const [messages, setMessages] = useState(() => {
        return JSON.parse(localStorage.getItem("messages")) || [];
    });

    useEffect(() => {
        console.log("saved messages");
        console.log(messages);
        localStorage.setItem("messages", JSON.stringify(messages));
    }, [messages]);
    
    useEffect(() => {
        const conn = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:44327/testhub")
            .withAutomaticReconnect([0, 2000, 5000, 10000])
            .build();

        conn.start()
            .then(() => console.log("Connection started"))
            .catch(err => console.error("SignalR error:", err));

        conn.on("ReceiveMessage", (message) => {
            console.log(message)
            setMessages(prev => [...prev, message]);
        });

        conn.onreconnecting(error => {
            console.log("Reconnecting...", error);
        });

        conn.onreconnected(connectionId => {
            console.log("Reconnected. ConnectionId:", connectionId);
        });

        conn.onclose(error => {
            console.error("Connection closed", error);
        });
        
        setConnection(conn);

        return () => conn.stop();
    }, []);

    const sendMessage = async () => {
        if (connection && inputText.trim() !== "") {
            await connection.invoke("SendMessage", inputText);
            setInputText("");
        }
    };

    return (
        <div className="flex flex-col items-center gap-x-2">
            <div className="flex space-x-2">
                <input
                    type="text"
                    value={inputText}
                    onChange={(e) => setInputText(e.target.value)}
                    placeholder="Enter message..."
                    className="input input-bordered w-full max-w-xs"
                />
                <button
                    onClick={sendMessage}
                    className="btn btn-primary"
                >
                    Send
                </button>
            </div>
            <div className="list-disc ml-5">
                <h3>Messages:</h3>
                <ul>
                    {messages.map((msg, i) => (
                        <li className="flex justify-between items-center p-3 bg-base-200 rounded-lg shadow" key={i}>{msg}</li>
                    ))}
                </ul>
            </div>
        </div>
    )
}