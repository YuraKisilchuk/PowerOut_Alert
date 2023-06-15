import qs from "qs";
import { ChangeEvent, useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import http from "../../../../http";
import GenericTable from "../../../common/table";
import { filterNonNull, IExclusionResult, IExclusionSearch } from "../types";

const countOnPage = 10;

const ExclusionsListPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const [search, setSearch] = useState<IExclusionSearch>({
    search: searchParams.get("search") || "",
    area: searchParams.get("area") || "",
    page: searchParams.get("page") || 1,
    sort: searchParams.get("sort") || "",
    countOnPage: searchParams.get("countOnPage") || countOnPage,
  });
  const [data, setData] = useState<IExclusionResult>({
    pages: 0,
    exclusions: [],
    total: 0,
    currentPage: 0,
  });

  useEffect(() => {
    http
      .get<IExclusionResult>("/api/exclusions", {
        params: search,
      })
      .then((resp) => {
        console.log("resp ", resp.data);
        setData(resp.data);
      });
  }, [search]);

  const deleteHandler = (id: number) => {
    http.delete("/api/exclusions/" + id).then((resp) => {
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
    setSearch({ ...search, search: '', page: 1 });
    setSearchParams(
      "?" + qs.stringify(filterNonNull({ ...search, search: '', page: 1 }))
    );
  };

  const onCountOnPageHandler = (value: number) => {
    setSearch({ ...search, countOnPage: value, page: 1 });
    setSearchParams(
      "?" + qs.stringify(filterNonNull({ ...search, countOnPage: value, page: 1 }))
    );
  };


  return (
    <>
      <GenericTable
        lenght={data.total}
        tableName={"Відключення"}
        list={data.exclusions.map((item) => {
          return {
            '#': item.id,
            'Назва': item.name.substring(0, 27) + "...",
            'Місто': item.city,
            'Початок': item.beginExclusion,
            'Кінець': item.endExclusion,
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

export default ExclusionsListPage;
