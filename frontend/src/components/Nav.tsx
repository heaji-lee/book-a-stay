import { useEffect, useState } from 'react'
import logo from '../assets/logo.png'
import type { Hotel } from '../models/hotel'

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL

export default function Nav() {
    const [hotels, setHotels] = useState<Hotel[]>([])

    useEffect(() => {
        fetch(`${apiBaseUrl}/api/hotels`)
            .then(response => {
                if (!response.ok) throw new Error('Could not load hotels')
                return response.json()
            })
            .then(setHotels)
            .catch(() => setHotels([]))
    }, [])

    return (
        <div className="sticky top-0 z-50 flex items-center p-5 gap-6 justify-between">
            <div>
                <img src={logo} alt="Book a Stay" className='w-20 h-20' />
            </div>

            <div>
                <h1>Book A Stay </h1>
            </div>

            <button className="btn" onClick={() => (document.getElementById('searchModal') as HTMLDialogElement | null)?.showModal()}>
                Get Available Rooms
            </button>
            <dialog id="searchModal" className="modal">
                <div className="modal-box max-w-2xl border border-slate-200 bg-white p-0 shadow-2xl">
                    <form method="dialog">
                        <button aria-label="Close search" className="btn btn-sm btn-circle btn-ghost absolute right-4 top-4 text-slate-500 hover:bg-slate-100">✕</button>
                    </form>
                    <div className="border-b border-slate-200  px-6 py-7 sm:px-8 bg-slate-50">
                        <p className="mb-1 text-sm font-semibold uppercase tracking-widest text-[#fbb726]">Plan your stay</p>
                        <h3 className="text-2xl font-bold text-[#122c5e]">Find an available room</h3>
                        <p className="mt-2 text-sm text-slate-500">Choose your dates and hotel to see the best options.</p>
                    </div>
                    <div className="grid gap-5 px-6 py-7 sm:grid-cols-3 sm:px-8 bg-slate-50">
                        <label className="form-control sm:col-span-3">
                            <span className="label-text mb-2 font-semibold text-[#122c5e]">Hotel</span>
                            <select defaultValue="Choose a hotel (optional)" className="select select-bordered w-full">
                                <option>Choose a hotel (optional)</option>
                                {hotels.map(hotel => (
                                    <option key={hotel.id} value={hotel.name}>{hotel.name}</option>
                                ))}
                            </select>
                        </label>
                        <label className="form-control">
                            <span className="label-text mb-2 font-semibold text-[#122c5e]">Check-in</span>
                            <input type="date" className="input input-bordered w-full" />
                        </label>
                        <label className="form-control">
                            <span className="label-text mb-2 font-semibold text-[#122c5e]">Check-out</span>
                            <input type="date" className="input input-bordered w-full" />
                        </label>
                    </div>
                    <div className="flex justify-end gap-3 border-t border-slate-200 px-6 py-4 sm:px-8">
                        <form method="dialog">
                            <button className="btn btn-ghost">Cancel</button>
                        </form>
                        <button className="btn bg-[#122c5e] text-white hover:bg-[#030f25]">Search rooms</button>
                    </div>
                </div>
                <form method="dialog" className="modal-backdrop"><button>close</button></form>
            </dialog>
        </div>
    )
}