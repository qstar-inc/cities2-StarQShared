export const customizeTabSections =
  (sectionName: string) =>
  (sections: Set<string>): Set<string> => {
    sections.add(sectionName);
    return sections;
  };
