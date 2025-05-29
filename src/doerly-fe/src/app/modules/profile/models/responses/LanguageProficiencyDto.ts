import { LanguageDto } from './LanguageDto';
import { ELanguageProficiencyLevel } from '../enums/ELanguageProficiencyLevel';

export interface LanguageProficiencyDto {
  id: number;
  language: LanguageDto;
  level: ELanguageProficiencyLevel;
} 