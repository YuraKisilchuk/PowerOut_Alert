import { ReactElement } from "react";

export interface ICityTableItem {
  id: number;
  name: string;
  area: string;
}

export interface ICreateCity {
  name: string;
  area: number;
}

export interface IEditCity {
  name: string;
  area: number;
}

export interface IAreaValue {
  name: string;
  id: number;
}


export interface ICitySearch {
    page?: number | string | null;
    search?: string;
    area?: string;
    sort?: string;
    countOnPage?: number | string | null;
  }

  export interface ICity {
    id: number;
    name: string;
    area: string;
  }

  export interface ICityResult {
    cities: Array<ICity>;
    pages: number;
    currentPage: number;
    total: number;
  }

  export function filterNonNull(obj: ICitySearch) {
    return Object.fromEntries(Object.entries(obj).filter(([k, v]) => v));
  }