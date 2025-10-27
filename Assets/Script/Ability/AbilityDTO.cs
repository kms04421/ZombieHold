using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AbilityDTO 
{
    public string Name; // 이름
    public string Description; //설명
    public string AbilityType; //적용할 어빌리티 타입
    public string AbilityStatus; //어빌리트 능력치
    public AbilityDTO(AbilityDTO abilityDTO)
    {
        Name = abilityDTO.Name;
        Description = abilityDTO.Description;
        AbilityType = abilityDTO.AbilityType;
        AbilityStatus = abilityDTO.AbilityStatus;
    }
}
[System.Serializable]
public class AbilityListWrapper
{
    public List<AbilityDTO> ability;
}