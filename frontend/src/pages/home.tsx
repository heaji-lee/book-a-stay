import { useEffect, useState } from "react";
import type { Hotel } from "../models/hotel";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

function Home() {
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
        <>
            <h1 className="bg-amber-400 p-8 text-4xl font-bold text-black">
                Tailwind is workingd
            </h1>
            <h1 className="bg-amber-400">Hotels</h1>
            {error && <p>{error}</p>}
            <ul>
                {hotels.map(hotel => (
                    <li key={hotel.id}>{hotel.name}</li>
                ))}
            </ul>
        </>
    );
}

export default Home;