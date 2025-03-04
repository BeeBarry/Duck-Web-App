import React from 'react';
import { Duck } from '../types';

// Uppdatera pathways när jag vet vart bilderna faktiskt ligger
import daVinciiDuck from '../assets/da-vinci-duck.webp';
import alanDuck from '../assets/bruce-duck.webp';

// Mappa ankans ID till rätt bild
const duckImages: Record<number, string> = {
    1: daVinciiDuck,
    2: alanDuck,
};

// Fallbackbild om ID inte matchar någon bild
const defaultDuckImage = '/default-duck.png';

interface DuckSelectorProps {
    ducks: Duck[];
    selectedDuckId: number | null;
    onSelect: (duckId: number) => void;
}

const DuckSelector: React.FC<DuckSelectorProps> = ({
                                                       ducks,
                                                       selectedDuckId,
                                                       onSelect
                                                   }) => {
    return (
        <div className="mb-8">
            <h2 className="text-xl font-bold uppercase mb-4"></h2>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {ducks.map((duck) => (
                    <div
                        key={duck.id}
                        onClick={() => onSelect(duck.id)}
                        className={`
                            cursor-pointer
                            bg-white border-3 border-black rounded-xl
                            transform transition-all duration-200
                            ${selectedDuckId === duck.id
                            ? 'ring-4 ring-[#FF3B3B] shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)]'
                            : 'shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)] hover:translate-y-1 hover:translate-x-1 hover:shadow-[2px_2px_0px_0px_rgba(0,0,0,0.8)]'
                        }
                        `}
                    >
                        <div className="aspect-square overflow-hidden">
                            <img
                                src={duckImages[duck.id] || defaultDuckImage}
                                alt={`${duck.name} anka`}
                                className="w-full h-full object-cover"
                            />
                        </div>

                        <div className="p-4 border-t-3 border-black">
                            <h3 className="font-bold text-lg mb-1">{duck.name}</h3>
                            <p className="text-sm text-gray-700 mb-2">{duck.specialty}</p>
                            <p className="text-xs italic text-gray-600">"{duck.motto}"</p>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default DuckSelector;