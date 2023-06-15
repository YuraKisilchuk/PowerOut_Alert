import classNames from "classnames";
import { useFormik } from "formik";
import { ChangeEvent, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import http from "../../../../http";
import Alert from "../../../common/alert";
import { FormField } from "../../../common/inputs/FormField";
import { ICreateArea } from "./types";
import { CategoryCreateSchema } from "./validation";
import defaultImage from "../../../../assets/no-image.webp";
import citylandscape from "../../../../assets/city-landscape.png";
import { FileField } from "../../../common/inputs/FileField";
import {
  ArchiveBoxXMarkIcon,
  PlusCircleIcon,
} from "@heroicons/react/24/outline";

const CreateArea = () => {
  const [alertOpen, setAlertOpen] = useState<boolean>(false);
  const navigator = useNavigate();

  const onSubmitFormik = async (values: ICreateArea) => {
    try {
      
      const result = await http.post("/api/areas", values);

      navigator('..');
    } catch (error: any) {
      setAlertOpen(true);
    }
  };

  const onFileChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
    const { target } = e;
    const { files } = target;
    if (files) {
      const file = files[0];
      setFieldValue("image", file);
    }
    target.value = "";
  };

  const initValues: ICreateArea = {
    name: "",
  };

  const formik = useFormik({
    initialValues: initValues,
    validationSchema: CategoryCreateSchema,
    onSubmit: onSubmitFormik,
  });

  const { values, errors, touched, handleSubmit, handleChange, setFieldValue } =
    formik;

  return (
    <>
      <section className="flex items-center justify-center m-2 max-w-lg mx-auto overflow-hidden bg-white rounded-lg shadow-lg dark:bg-gray-800 lg:max-w-7xl">
        <div className="flex justify-center">
          <img
            className="hidden bg-cover lg:block lg:w-2/5"
            src={citylandscape}
          />

          <div className="flex items-center w-full max-w-3xl p-8 mx-auto lg:px-12 lg:w-3/5">
            <div className="w-full">
              <Alert
                text={"Упс... Щось пішло не так! Попробуйте пізніше."}
                type={"danger"}
                open={alertOpen}
                setOpen={setAlertOpen}
              />
              <h1 className="mt-2 text-2xl font-semibold tracking-wider text-gray-800 capitalize dark:text-white">
                Додати область
              </h1>

              <form
                className="grid grid-cols-1 gap-6 mt-8 md:grid-cols-2"
                onSubmit={handleSubmit}
              >

                <div className="col-span-2">
                  <FormField
                    onChange={handleChange}
                    value={values.name}
                    label={"Назва"}
                    placeholder={"Назва"}
                    field={"name"}
                    error={errors.name}
                    touched={touched.name}
                  />
                </div>

                <button
                  type="submit"
                  className="lg:col-span-1 sm:col-span-2 flex items-center justify-between w-full px-6 py-3 text-sm tracking-wide text-white capitalize transition-colors duration-300 transform bg-blue-500 rounded-lg hover:bg-blue-400 focus:outline-none focus:ring focus:ring-blue-300 focus:ring-opacity-50"
                >
                  <span>Добавити </span>

                  <PlusCircleIcon className="w-5 h-5 rtl:-scale-x-100" />
                </button>
                <Link
                  to=".."
                  className="lg:col-span-1 sm:col-span-2 flex items-center justify-between w-full px-6 py-3 text-sm tracking-wide text-white capitalize transition-colors duration-300 transform bg-gray-600 rounded-lg hover:bg-gray-700 focus:outline-none focus:ring focus:ring-blue-300 focus:ring-opacity-50"
                >
                  <span>Назад </span>

                  <ArchiveBoxXMarkIcon className="w-5 h-5 rtl:-scale-x-100" />
                </Link>
              </form>
            </div>
          </div>
        </div>
      </section>
    </>
  );
};
export default CreateArea;
