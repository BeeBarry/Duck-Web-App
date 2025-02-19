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
            label: "Visdom",
            bgColorSelected: "#e6f7ff"
        },
        [QuoteType.Comical]: {
            icon: "😂",
            label: "Humor",
            bgColorSelected: "#fff7e6"
        },
        [QuoteType.Dark]: {
            icon: "🖤",
            label: "Mörkt",
            bgColorSelected: "#f9f0ff"
        }
    };

    return (
        <div className="flex gap-6">
            {Object.values(QuoteType).map((type) => (
                <button
                    key={type}
                    onClick={() => onSelect(type)}
                    className={`
                        flex-1 py-4 px-3 rounded-lg
                        border-3 border-black
                        font-bold text-black
                        shadow-[4px_4px_0px_0px_rgba(0,0,0,0.8)]
                        ${selectedType === type
                        ? `bg-[${typeConfig[type].bgColorSelected}]`
                        : 'bg-white hover:bg-gray-100'
                    }
                    `}
                    style={{
                        backgroundColor: selectedType === type
                            ? typeConfig[type].bgColorSelected
                            : 'white'
                    }}
                >
                    <div className="flex flex-col items-center justify-center">
                        <span className="text-2xl mb-2">{typeConfig[type].icon}</span>
                        <span>{typeConfig[type].label}</span>
                    </div>
                </button>
            ))}
        </div>
    );
};

export default QuoteTypeSelector;