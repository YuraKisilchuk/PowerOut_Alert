import { ReactElement } from "react";

export interface IExclusionTableItem {
  id: number;
  name: string;
  area: string;
}

export interface ICreateExclusion {
  name: string;
  area: number;
}

export interface IEditExclusion {
  name: string;
  area: number;
}

export interface ICityValue {
  name: string;
  id: number;
  area: string;
}


export interface IExclusionSearch {
    page?: number | string | null;
    search?: string;
    area?: string;
    sort?: string;
    countOnPage?: number | string | null;
  }

  export interface IExclusion {
    id: number;
    name: string;
    city: string;
    beginExclusion: string;
    endExclusion: string;
  }

  export interface IExclusionResult {
    exclusions: Array<IExclusion>;
    pages: number;
    currentPage: number;
    total: number;
  }

  export function filterNonNull(obj: IExclusionSearch) {
    return Object.fromEntries(Object.entries(obj).filter(([k, v]) => v));
  }