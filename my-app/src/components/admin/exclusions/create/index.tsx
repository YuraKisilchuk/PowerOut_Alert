import Alert from "../../../common/alert";
import citylandscape from "../../../../assets/city-landscape.png";
import { useEffect, useState } from "react";
import { ICreateExclusion } from "./types";
import http from "../../../../http";
import { Link, useNavigate } from "react-router-dom";
import { useFormik } from "formik";
import { CreateExclusionSchema } from "./validation";
import {
  ArchiveBoxXMarkIcon,
  PlusCircleIcon,
} from "@heroicons/react/24/outline";
import { FormField } from "../../../common/inputs/FormField";
import { SelectField } from "../../../common/inputs/SelectField";
import { ICityValue } from "../types";

const CreateExclusion = () => {
  const [alertOpen, setAlertOpen] = useState<boolean>(false);
  const [cities, setCities] = useState<ICityValue[]>([]);
  const navigator = useNavigate();

  var m = new Date();
  var dateString = m.getUTCFullYear() +"."+ (m.getUTCMonth()+1) +"."+ m.getUTCDate();

  const initValues: ICreateExclusion = {
    name: "",
    cityId: 1,
    beginExclusion: dateString+" 14:00",
    endExclusion: dateString+" 18:00"
  };

  const onSubmitFormik = async (values: ICreateExclusion) => {
    try {
      console.log("Reqest server", values);
      const result = await http.post("/api/exclusions", values);
      
      navigator('..');
    } catch (error: any) {
       setAlertOpen(true);
    }
  };

  useEffect(() => {
    http.get("/api/cities/selector").then((resp) => {
      setCities(resp.data);
    });
  }, []);
  

  const formik = useFormik({
    initialValues: initValues,
    validationSchema: CreateExclusionSchema,
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
                Добавити відлкючення
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

                <div className="col-span-2">
                  <FormField
                    onChange={handleChange}
                    value={values.beginExclusion}
                    label={"Дата початку відключення"}
                    placeholder={"Дата відклчюення"}
                    field={"beginExclusion"}
                    error={errors.beginExclusion}
                    touched={touched.beginExclusion}
                  />
                </div>

                <div className="col-span-2">
                  <FormField
                    onChange={handleChange}
                    value={values.endExclusion}
                    label={"Дата кінця відключення"}
                    placeholder={"Дата відклчюення"}
                    field={"endExclusion"}
                    error={errors.endExclusion}
                    touched={touched.endExclusion}
                  />
                </div>

                <div className="col-span-2">
                  <SelectField
                    onChange={handleChange}
                    value={values.cityId}
                    label={"Місто"}
                    field={"cityId"}
                    error={errors.cityId}
                    touched={touched.cityId}
                    items={cities.map((item) => {
                      return { label: `${item.name} (${item.area})`, value: item.id };
                    })}
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
export default CreateExclusion;
