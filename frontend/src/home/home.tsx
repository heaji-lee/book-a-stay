import { useEffect, useState } from "react";

type Hotel = {
    id: number;
    name: string;
};

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

function App() {
    const [hotels, setHotels] = useState<Hotel[]>([]);
    const [error, setError] = useState("");

    useEffect(() => {
        fetch(`${apiBaseUrl}/api/hotels`)
            .then(response => {
                if (!response.ok) throw new Error("Could not load hotels");
                return response.json();
            })
            .then(setHotels)
            .catch(error => setError(error.message));
    }, []);

    return (
        <main>
            <h1>Hotels</h1>
            {error && <p>{error}</p>}
            <ul>
                {hotels.map(hotel => (
                    <li key={hotel.id}>{hotel.name}</li>
                ))}
            </ul>
        </main>
    );
}

export default App;