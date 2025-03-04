import http from "k6/http";
import { check } from "k6";

export const options = {
    stages: [
        { duration: "30s", target: 100 },
        { duration: "1m", target: 100 },
        { duration: "30s", target: 0 },
    ],
};

export default function () {
    const response = http.get(`http://localhost:5000/api/demo/output-cache/Id_${Math.floor(Math.random() * 3000)}`);

    check(response, {
        "is status success": (r) => r.status >= 200 && r.status <= 299,
    });
}
