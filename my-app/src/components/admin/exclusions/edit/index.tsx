import {
    ArchiveBoxXMarkIcon,
    PencilIcon,
  } from "@heroicons/react/24/outline";
  import { useFormik } from "formik";
  import { useEffect, useState } from "react";
  import { Link, useNavigate, useParams } from "react-router-dom";
  import { APP_ENV } from "../../../../env";
  import http from "../../../../http";
  import Alert from "../../../common/alert";
  import { FormField } from "../../../common/inputs/FormField";
  import { SelectField } from "../../../common/inputs/SelectField";
  import { TinyEditor } from "../../../common/inputs/TinyEditor";
  import { ICityValue } from "../types";
  import { IEditExclusion } from "./types";
  import { EditExclusionSchema } from "./validation";
  import noimage from "../../../../assets/no-image.webp";
  import citylandscape from "../../../../assets/amazing-city-landscape.png";
  
  const EditExclusion = () => {
    const [alertOpen, setAlertOpen] = useState<boolean>(false);
    const [areas, setAreas] = useState<ICityValue[]>([]);
    const navigator = useNavigate();
    const { id } = useParams();
  
    const initValues: IEditExclusion = {
      id: id || "",
      name: "",
      cityId: 0,
    };
  
    const onSubmitFormik = async (values: IEditExclusion) => {
      try {
        console.log("submit");

        const result = await http.put("/api/exclusion", values);

        navigator("../..");
      } catch (error: any) {
        setAlertOpen(true);
      }
    };
  
    useEffect(() => {
      http.get("/api/cities/selector").then((resp) => {
        setAreas(resp.data);
        const areas = resp.data;
        http
        .get("/api/exclusions/id/" + id)
        .then((resp) => {
          const { data } = resp;
          setFieldValue("name", data.name);
          setFieldValue("areaId", areas.filter((item:any)=>item.name == data.area)[0].id);
        })
        .catch((error) => {
          navigator("/control-panel/error404");
        });
      });
    }, [id]);
  
   
    
  
    const formik = useFormik({
      initialValues: initValues,
      validationSchema: EditExclusionSchema,
      onSubmit: onSubmitFormik,
    });
  
    const { values, errors, touched, handleSubmit, handleChange, setFieldValue } =
      formik;
  
    return (
      <>
        <section className="flex items-center justify-center m-2 max-w-lg mx-auto overflow-hidden bg-white rounded-lg shadow-lg dark:bg-gray-700 lg:max-w-7xl">
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
                  Редагувати відключення.
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
                    <SelectField
                      onChange={handleChange}
                      value={values.cityId}
                      label={"Місто"}
                      field={"cityId"}
                      error={errors.cityId}
                      touched={touched.cityId}
                      items={areas.map((item) => {
                        return { label: item.name, value: item.id };
                      })}
                    />
                  </div>
  
  
                  <button
                    type="submit"
                    className="lg:col-span-1 sm:col-span-2 flex items-center justify-between w-full px-6 py-3 text-sm tracking-wide text-white capitalize transition-colors duration-300 transform bg-blue-500 rounded-lg hover:bg-blue-400 focus:outline-none focus:ring focus:ring-blue-300 focus:ring-opacity-50"
                  >
                    <span>Редагувати </span>
  
                    <PencilIcon className="w-5 h-5 rtl:-scale-x-100" />
                  </button>
                  <Link
                    to="../.."
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
  export default EditExclusion;
  