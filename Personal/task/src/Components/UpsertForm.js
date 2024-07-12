import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import "bootstrap/dist/css/bootstrap.min.css";
import { GetLanguages, GetRaces, UpsertFormData } from "../Api";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { MultiSelect } from "react-multi-select-component";
import InputMask from "react-input-mask";
import LoadingSpinner from "./LoadingSpinner"; // Import the LoadingSpinner component

export default function UpsertForm() {
  const [allLanguages, setAllLanguages] = useState([]);
  const [allRaces, setAllRaces] = useState([]);
  const [selected, setSelected] = useState([]);

  const [loading, setLoading] = useState(false); // State to manage loading state

  useEffect(() => {
    setLoading(true);
    GetLanguages().then((response) => {
      if (response) {
        console.log("Success Response : ", response);
        setAllLanguages(response);
      } else {
        console.log("Error: ", response);
      }
    }).finally(() => setLoading(false));

    GetRaces().then((response) => {
      if (response) {
        console.log("Success Response : ", response);
        setAllRaces(response);
      } else {
        console.log("Error: ", response);
      }
    }).finally(() => setLoading(false));
  }, []);

  const options = allLanguages.map((language) => ({
    label: language.name,
    value: language.name,
  }));

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm();

  const onSubmit = async (data) => {
    setLoading(true);
   
    const languageNames = selected.map((language) => language.value);
    
    try {
      const response = await UpsertFormData(data, languageNames);
      if (response === "PersonalInfo added successfully") {
        console.log("Success Response (Form Submit): ", response);
        reset();
        toast.success("PersonalInfo added successfully");
      } else {
        console.log("Error: ", response);
        toast.error("Personal information with the same email already exists");
      }
    } catch (error) {
      console.error("Error submitting form:", error);
      toast.error("Failed to submit form. Please try again.");
    } finally {
      setLoading(false);
      // Enable form after 3 seconds
      setTimeout(() => {
      }, 9000);
    }
  };
  if (loading) {
    return <LoadingSpinner message="Loading..." />;
  }
  return (
    <div className="p-5 w-80" style={{ backgroundColor: "#f0f0f0" }}>
      <div className="row">
        <h1>Add Form</h1>
      </div>
      <form onSubmit={handleSubmit(onSubmit)}>

      {loading && <LoadingSpinner message="Loading..." />}
        <div className="row mt-3">
          <div className="col-6">
            <label>First Name</label>
            <input
              className="form-control"
              {...register("firstName", { required: true })}
            />
            {errors.firstName && (
              <span className="text-danger">This field is required</span>
            )}
          </div>

          <div className="col-6">
            <label>Last Name</label>
            <input
              className="form-control"
              {...register("lastName", { required: true })}
            />
            {errors.lastName && (
              <span className="text-danger">This field is required</span>
            )}
          </div>
        </div>
        <div className="row mt-3">
          <div className="col-6">
            <label>Email</label>
            <input
              className="form-control"
              {...register("email", { required: true, pattern: /^\S+@\S+$/i })}
            />
            {errors.email && (
              <span className="text-danger">Valid email is required</span>
            )}
          </div>

          <div className="col-6">
            <label>Mobile No</label>
            <InputMask
              mask="(999) 999-9999"
              className="form-control"
              {...register("mobile", {
                required: "Valid mobile number is required",
                pattern: {
                  value: /^\(\d{3}\) \d{3}-\d{4}$/,
                  message: "Invalid mobile number format",
                },
              })}
            />
            {errors.mobile && (
              <span className="text-danger">{errors.mobile.message}</span>
            )}
          </div>
        </div>
        <div className="row mt-3">
          <div className="col-6">
            <label>Address</label>
            <input
              className="form-control"
              {...register("address", { required: true })}
            />
            {errors.address && (
              <span className="text-danger">This field is required</span>
            )}
          </div>

          <div className="col-6">
            <label>City</label>
            <input
              className="form-control"
              {...register("city", { required: true })}
            />
            {errors.city && (
              <span className="text-danger">This field is required</span>
            )}
          </div>
        </div>
        <div className="row mt-3">
          <div className="col-6">
            <label>State</label>
            <input
              className="form-control"
              {...register("state", { required: true })}
            />
            {errors.state && (
              <span className="text-danger">This field is required</span>
            )}
          </div>

          <div className="col-6">
            <label>Zip Code</label>
            <input
              type="text"
              className="form-control"
              {...register("zip", { required: true, pattern: /^[0-9]+$/i })}
            />
            {errors.zip && (
              <span className="text-danger">Valid zip code is required</span>
            )}
          </div>
        </div>
        <div className="row mt-3">
          <div className="col-6">
            <label>Marital Status</label>
            <select
              className="form-control"
              {...register("maritalStatus", { required: true })}
            >
              <option value="">Select...</option>
              <option value="single">Single</option>
              <option value="married">Married</option>
              <option value="divorced">Divorced</option>
              <option value="widowed">Widowed</option>
            </select>
            {errors.maritalStatus && (
              <span className="text-danger">This field is required</span>
            )}
          </div>
          <div className="col-6">
            <label>Languages</label>
            <MultiSelect
              options={options}
              value={selected}
              onChange={setSelected}
              labelledBy="Select"
            />
            {errors.languages && (
              <span className="text-danger">This field is required</span>
            )}
          </div>
        </div>
        <div className="row mt-3">
          <div className="col-6">
            <label>Gender</label>{" "}
            <div className=" form-check form-check-inline">
              <input
                className="form-check-input"
                type="radio"
                value="male"
                {...register("gender", { required: true })}
              />
              <label className="form-check-label">Male</label>
            </div>
            <div className="form-check form-check-inline">
              <input
                className="form-check-input"
                type="radio"
                value="female"
                {...register("gender", { required: true })}
              />
              <label className="form-check-label">Female</label>
            </div>
            {errors.gender && (
              <span className="text-danger d-block">
                This field is required
              </span>
            )}
          </div>

          <div className="col-6">
            <label>Race</label>
            <select
              className="form-control"
              {...register("race", { required: true })}
            >
              <option value="">Select...</option>
              {allRaces.map((race) => (
                <option key={race.id} value={race.name}>
                  {race.name}
                </option>
              ))}
            </select>
            {errors.race && (
              <span className="text-danger">This field is required</span>
            )}
          </div>
        </div>
        <div className="row mt-3">
          <div className="col-12">
            <input className="btn btn-primary" type="submit" />
          </div>
        </div>
      </form>
    </div>
  );
}
