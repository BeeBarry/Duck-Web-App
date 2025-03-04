import React from 'react';
import { QuoteType } from '../types';

interface QuoteTypeSelectorProps {
    selectedType: QuoteType | null;
    onSelect: (type: QuoteType) => void;
}

const QuoteTypeSelector: React.FC<QuoteTypeSelectorProps> = ({
                                                                 selectedType,
                                                                 onSelect
                                                             }) => {
    // Testar emojis som design
    const typeConfig = {
        [QuoteType.Wise]: {
            icon: "🧠",
            label: "Visdom"
        },
        [QuoteType.Comical]: {
            icon: "😂",
            label: "Humor"
        },
        [QuoteType.Dark]: {
            icon: "🖤",
            label: "Mörkt"
        }
    };

    return (
        <div className="mb-8">
            <h2 className="text-xl font-bold uppercase mb-4">Välj typ av citat</h2>

            <div className="grid grid-cols-3 gap-4 md:gap-6">
                {Object.values(QuoteType).map((type) => (
                    <button
                        key={type}
                        onClick={() => onSelect(type)}
                        className={`
                            py-4 px-2 rounded-lg
                            border-3 border-black
                            font-bold text-black
                            shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)]
                            transform transition-all duration-200
                            ${selectedType === type
                            ? 'bg-[#FF3B3B] text-white'
                            : 'bg-white hover:bg-gray-100 hover:translate-y-1 hover:translate-x-1 hover:shadow-[2px_2px_0px_0px_rgba(0,0,0,0.8)]'
                        }
                        `}
                    >
                        <div className="flex flex-col items-center justify-center">
                            <span className="text-2xl mb-2">{typeConfig[type].icon}</span>
                            <span>{typeConfig[type].label}</span>
                        </div>
                    </button>
                ))}
            </div>
        </div>
    );
};

export default QuoteTypeSelector;