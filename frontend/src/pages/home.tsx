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
            <div>
                {error && <p>{error}</p>}
                <ul>
                    {hotels.map(hotel => (
                        <div>
                            <li key={hotel.id}>{hotel.name}</li>
                            <div className="hover-3d">
                                <figure className="w-60 rounded-2xl">
                                    <img src={`${apiBaseUrl}${hotel.imageUrl}`} />
                                </figure>
                                <div></div>
                                <div></div>
                                <div></div>
                                <div></div>
                                <div></div>
                                <div></div>
                                <div></div>
                                <div></div>
                            </div>

                        </div>
                    ))}
                </ul>
            </div>
        </>
    );
}

export default Home;