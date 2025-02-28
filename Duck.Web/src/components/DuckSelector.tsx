import React from 'react';
import { Duck } from '../types';

// Uppdatera pathways när jag vet vart bilderna faktiskt ligger
import daVinciiDuck from '../assets/Da-vinci-duck.webp';
import bruceDuck from '../assets/Bruce-duck.webp';

// Mappa ankans ID till rätt bild
const duckImages: Record<number, string> = {
    1: daVinciiDuck,
    2: bruceDuck,
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
        <div className="grid grid-cols-2 gap-6">
            {ducks.map((duck) => (
                <div
                    key={duck.id}
                    onClick={() => onSelect(duck.id)}
                    className={`
                        cursor-pointer overflow-hidden
                        bg-white border-3 border-black rounded-lg
                        ${selectedDuckId === duck.id
                        ? 'bg-gray-100'
                        : 'shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)]'
                    }
                    `}
                >
                    <div className="relative pb-[80%] overflow-hidden">
                        <img
                            src={duckImages[duck.id] || defaultDuckImage}
                            alt={`${duck.name} anka`}
                            className="absolute inset-0 w-full h-full object-cover"
                        />
                    </div>

                    <div className="p-5 border-t-3 border-black">
                        <h3 className="font-bold text-lg mb-2">{duck.name}</h3>
                        <p className="text-md mb-3">{duck.specialty}</p>
                        <p className="text-sm italic mt-1">"{duck.motto}"</p>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default DuckSelector;