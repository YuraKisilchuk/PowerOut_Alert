import React from "react";
import logo from "./logo.svg";
import "./App.css";
import { Route, Routes } from "react-router-dom";
import LoginPage from "./components/auth/login";
import MainLayout from "./components/containers/Layout/default";
import Error404 from "./components/error/404";
import ControlPanelLayout from "./components/containers/Layout/admin";
import AdminHomePage from "./components/admin/home";
import { Logout } from "./components/auth/Logout";
import AreasListPage from "./components/admin/areas/list";
import CreateArea from "./components/admin/areas/create";
import EditArea from "./components/admin/areas/edit";
import CitiesListPage from "./components/admin/cities/list";
import CreateCity from "./components/admin/cities/create";
import EditCity from "./components/admin/cities/edit";
import ExclusionsListPage from "./components/admin/exclusions/list";
import CreateExclusion from "./components/admin/exclusions/create";
import EditExclusion from "./components/admin/exclusions/edit";
import TelegramChatsListPage from "./components/admin/telegramChats/list";

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<MainLayout />}>
          <Route index element={<LoginPage />} />
          <Route path="logout" element={<Logout />} />
          <Route path="*" element={<Error404 />} />
        </Route>
        <Route path="/control-panel" element={<ControlPanelLayout />}>
          <Route index element={<AdminHomePage />} />
          <Route path="areas">
            <Route index element={<AreasListPage />} />
            <Route path="create" element={<CreateArea />} />
            <Route path="edit">
              <Route path=":id" element={<EditArea />} />
            </Route>
          </Route>

          <Route path="cities">
            <Route index element={<CitiesListPage />} />
            <Route path="create" element={<CreateCity />} />
            <Route path="edit">
              <Route path=":id" element={<EditCity />} />
            </Route>
          </Route>

          <Route path="exclusions">
            <Route index element={<ExclusionsListPage />} />
            <Route path="create" element={<CreateExclusion />} />
            <Route path="edit">
              <Route path=":id" element={<EditExclusion />} />
            </Route>
          </Route>

          <Route path="telegramChats">
            <Route index element={<TelegramChatsListPage />} />
            {/* <Route path="create" element={<CreateExclusion />} />
            <Route path="edit">
              <Route path=":id" element={<EditExclusion />} />
            </Route>*/}
          </Route>
        </Route>
      </Routes>
    </>
  );
}

export default App;
