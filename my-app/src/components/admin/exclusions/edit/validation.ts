import * as yup from "yup";

const digitsOnly = (value: string | undefined) =>
  /^\d*[.{1}\d*]\d*$/.test(value || "") || (value || "").length === 0;

export const EditExclusionSchema = yup.object().shape({
  name: yup.string().required("Це обов'язкове поле!"),
  areaId: yup.number().required("Це обов'язкове поле!").integer(),
});