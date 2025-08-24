import React, { useState } from "react";
import {API} from "./api/common.jsx";

export default function Login() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [token, setToken] = useState(localStorage.getItem("jwt") || "");
    const [data, setData] = useState(null);

    const login = async () => {
        try {
            const response = await API.post("/auth/login", {
                username,
                password
            });
            const jwt = response.data.token;
            localStorage.setItem("jwt", jwt);
            setToken(jwt);
        } catch (err) {
            console.error("Login failed:", err);
        }
    };

    return (
        <div className="p-4">
            {!token ? (
                <div>
                    <input
                        type="text"
                        placeholder="Username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        className="input input-bordered m-1"
                    />
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        className="input input-bordered m-1"
                    />
                    <button onClick={login} className="btn btn-primary m-1">
                        Login
                    </button>
                </div>
            ) : (
                <div>
                    <h3 className="font-bold">Logged in with JWT</h3>
                    <button
                        onClick={() => {
                            localStorage.removeItem("jwt");
                            setToken("");
                        }}
                        className="btn btn-error m-1"
                    >
                        Logout
                    </button>
                </div>
            )}

            <div className="mt-4">
                <h4>Protected Data:</h4>
                {data ? (
                    <pre className="bg-gray-100 p-2 rounded">{JSON.stringify(data, null, 2)}</pre>
                ) : (
                    <p>No data yet.</p>
                )}
            </div>
        </div>
    );
}
