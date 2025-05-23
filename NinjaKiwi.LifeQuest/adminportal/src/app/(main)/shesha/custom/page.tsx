"use client";

import React from "react";
import { PageWithLayout } from "@shesha-io/reactjs";
import HealthAnalysisComponent from "../../../../components/food-scanner"; // adjust path if needed

interface IProps {}

const Gemini: PageWithLayout<IProps> = () => {
  return <HealthAnalysisComponent />;
};

export default Gemini;
