
export interface IAreaSearch {
    page?: number | string | null;
    search?: string;
    countOnPage?: number | string | null;
  }
  
  export interface IAreaResult {
    areas: Array<IAreaItem>;
    pages: number;
    currentPage: number;
    total: number;
  }

  export interface IAreaItem 
{
    id: number;
    name:string;
    // image:string;
}