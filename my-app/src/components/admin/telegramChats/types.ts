import { ReactElement } from "react";

export interface ITelegramChatTableItem {
  id: number;
  name: string;
  area: string;
}

export interface ICreateTelegramChat {
  name: string;
  area: number;
}

export interface IEditTelegramChat {
  name: string;
  area: number;
}

export interface ITelegramChatSearch {
  page?: number | string | null;
  search?: string;
  sort?: string;
  countOnPage?: number | string | null;
}

export interface ITelegramChat {
  id: number;
  chatId: string;
  firstName: string;
  lastName: string;
  username: string;
}

export interface ITelegramChatResult {
  telegramChats: Array<ITelegramChat>;
  pages: number;
  currentPage: number;
  total: number;
}

export function filterNonNull(obj: ITelegramChatSearch) {
  return Object.fromEntries(Object.entries(obj).filter(([k, v]) => v));
}
