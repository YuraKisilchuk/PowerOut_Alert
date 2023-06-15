import qs from "qs";
import { ChangeEvent, useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import http from "../../../../http";
import GenericTable from "../../../common/table";
import {
  filterNonNull,
  ITelegramChatResult,
  ITelegramChatSearch,
} from "../types";

const countOnPage = 10;

const TelegramChatsListPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const [search, setSearch] = useState<ITelegramChatSearch>({
    search: searchParams.get("search") || "",
    page: searchParams.get("page") || 1,
    sort: searchParams.get("sort") || "",
    countOnPage: searchParams.get("countOnPage") || countOnPage,
  });
  const [data, setData] = useState<ITelegramChatResult>({
    pages: 0,
    telegramChats: [],
    total: 0,
    currentPage: 0,
  });

  useEffect(() => {
    http
      .get<ITelegramChatResult>("/api/telegramChats", {
        params: search,
      })
      .then((resp) => {
        console.log("resp ", resp.data);
        setData(resp.data);
      });
  }, [search]);

  const deleteHandler = (id: number) => {
    http.delete("/api/telegramChats/" + id).then((resp) => {
      setSearch({ ...search, search: search.search });
    });
  };

  const onClickHandler = (page: number) => setSearch({ ...search, page });

  const onSearchHandler = (e: ChangeEvent<HTMLInputElement>) => {
    const { value } = e.target;
    setSearch({ ...search, search: value, page: 1 });
    setSearchParams(
      "?" + qs.stringify(filterNonNull({ ...search, search: value, page: 1 }))
    );
  };

  const onClearSearchHandler = () => {
    setSearch({ ...search, search: "", page: 1 });
    setSearchParams(
      "?" + qs.stringify(filterNonNull({ ...search, search: "", page: 1 }))
    );
  };

  const onCountOnPageHandler = (value: number) => {
    setSearch({ ...search, countOnPage: value, page: 1 });
    setSearchParams(
      "?" +
        qs.stringify(filterNonNull({ ...search, countOnPage: value, page: 1 }))
    );
  };

  return (
    <>
      <GenericTable
        lenght={data.total}
        tableName={"Користувачі"}
        list={data.telegramChats.map((item) => {
          return {
            "#": item.id,
            ChatId: item.chatId,
            Назва: item.username,
            Прізвище: item.lastName,
            "Ім'я": item.firstName,
          };
        })}
        search={search}
        pages={data.pages}
        countOnPage={search.countOnPage}
        currentPage={data.currentPage}
        onSearch={onSearchHandler}
        onPageChange={onClickHandler}
        onDelete={deleteHandler}
        onClearSearch={onClearSearchHandler}
        onCountOnPageChange={onCountOnPageHandler}
      />
    </>
  );
};

export default TelegramChatsListPage;
