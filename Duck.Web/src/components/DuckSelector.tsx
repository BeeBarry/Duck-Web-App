import React from 'react';
import { Duck } from '../types';


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

// Funktion för att välja kortfärg baserat på index
    const getCardColors = (index: number) => {
        if (index % 2 === 0) {
            return {
                outer: 'bg-blue-200',
                inner: 'bg-blue-300'
            };
        } else {
            return {
                outer: 'bg-green-200',
                inner: 'bg-green-300'
            };
        }
    };

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
            {ducks.map((duck, index) => {
                const colors = getCardColors(index);

                return (
                    <div
                        key={duck.id}
                        onClick={() => onSelect(duck.id)}
                        className={`
              ${colors.outer} 
              border-4 border-black 
              rounded-lg 
              px-8 pt-8 pb-8
              w-48
              min-h-56
              flex flex-col items-center
              cursor-pointer
              transition-all duration-200
              hover:scale-105
              ${selectedDuckId === duck.id ? 'ring-4 ring-orange-500' : ''}
            `}
                    >
                        {/* Cirkulär bildcontainer med inzoomad bild */}
                        <div className={`
              ${colors.inner} 
              w-30 h-30 
              rounded-full 
              border-4 border-black 
              overflow-hidden
              mt-2 mb-4
              flex items-center justify-center
            `}>
                            <div className="w-full h-full overflow-hidden">
                                <img
                                    src={duckImages[duck.id] || defaultDuckImage}
                                    alt={`${duck.name} anka`}
                                    className="w-full h-full object-cover object-center scale-100"
                                />
                            </div>
                        </div>

                        {/* Information om ankan */}
                        <h3 className="font-bold text-lg text-center">{duck.name}</h3>
                        <p className="text-sm text-center">{duck.specialty}</p>
                    </div>
                );
            })}
        </div>
    );
};

export default DuckSelector;