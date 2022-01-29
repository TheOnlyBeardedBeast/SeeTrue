import React from "react";
import { useForm } from "react-hook-form";
import { SignupRequest } from "seetrue.client";
import { useSeeTrue } from "seetrue.client.react";
import { useLocation } from "wouter";

export const SignUp: React.FC = () => {
  const [loading, setLoading] = React.useState(false);
  const [sent, setSent] = React.useState(false);
  const { client } = useSeeTrue();
  const [_location, setLocation] = useLocation();

  const { register, handleSubmit } = useForm({
    defaultValues: {
      email: "",
      password: "",
      name: "",
      language: "en",
    },
  });

  const onSubmit = async (data: any) => {
    try {
      setLoading(true);
      await client.signup({
        email: data.email,
        password: data.password,
        userMetaData: { Name: data.name },
        language: data.language,
      } as SignupRequest);
      setSent(true);
    } catch (error) {
      console.log(error);
    } finally {
      setLoading(false);
    }
  };

  const onLogin = () => {
    setLocation("/login");
  };

  if (sent) {
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
      }}
    >
      <span>Check your mail</span>
    </div>;
  }

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh",
      }}
    >
      <form
        onSubmit={handleSubmit(onSubmit)}
        style={{ display: "flex", flexDirection: "column", width: "400px" }}
      >
        <label htmlFor="email">Email</label>
        <input type="email" {...register("email")} />
        <label htmlFor="password">Password</label>
        <input type="password" {...register("password")} />
        <label htmlFor="name">Password</label>
        <input type="text" {...register("name")} />
        <label htmlFor="language">Password</label>
        <select {...register("language")}>
          <option value="en" defaultChecked>
            English
          </option>
        </select>
        <button type="submit" disabled={loading}>
          {loading ? "Loading" : "Sign up"}
        </button>
        <button onClick={onLogin} type="button">
          Login
        </button>
      </form>
    </div>
  );
};
