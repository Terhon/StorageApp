import {Navigate} from "react-router-dom";

export function isAuthenticated() {
    const token = localStorage.getItem("jwt");
    if (!token) 
        return false;

    try {
        const payload = JSON.parse(atob(token.split(".")[1]));
        const now = Date.now() / 1000;
        return payload.exp && payload.exp > now;
    } catch {
        return false;
    }
}

export function ProtectedRoute({ children }) {
    if (!isAuthenticated()) {
        return <Navigate to="/login" replace />;
    }
    return children;
}