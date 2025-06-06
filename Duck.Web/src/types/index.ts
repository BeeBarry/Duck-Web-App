export interface Duck {
    id: number;
    name: string;
    specialty: string;
    personality: string;
    motto: string;
}

export enum QuoteType {
    Wise = "Wise",
    Comical = "Comical",
    Dark = "Dark"
}

export interface RandomQuote {
    content: string;
    explanation?: string;
    duckName: string;
    type: QuoteType;
}

