using Application.Defects;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;
public class DefectMappingProfile : Profile
{
    public DefectMappingProfile()
    {
        CreateMap<Defect, DefectDto>();

        CreateMap<IEnumerable<IGrouping<DefectStatus, Defect>>, DefectStatusGroupsDto>()
            .ConvertUsing<DefectTypeConverter>();

        CreateMap<Defect, DefectStatusGroupEntryDto>()
            .ForMember(dest => dest.WorkerName, opt => opt.MapFrom(src => src.Worker.Name));
    }
}

public class DefectTypeConverter : ITypeConverter<IEnumerable<IGrouping<DefectStatus, Defect>>, DefectStatusGroupsDto>
{
    public DefectStatusGroupsDto Convert(IEnumerable<IGrouping<DefectStatus, Defect>> source, DefectStatusGroupsDto destination, ResolutionContext context)
    {
        return new DefectStatusGroupsDto(
            FilerDefectGroupByStatus(source, DefectStatus.Pending, context),
            FilerDefectGroupByStatus(source, DefectStatus.InProcess, context),
            FilerDefectGroupByStatus(source, DefectStatus.Completed, context));
    }

    public static IEnumerable<DefectStatusGroupEntryDto> FilerDefectGroupByStatus(
        IEnumerable<IGrouping<DefectStatus, Defect>> groups, 
        DefectStatus status, 
        ResolutionContext context)
    {
        return groups
            .First(g => g.Key == status)
            .Select(defect => context.Mapper.Map<Defect, DefectStatusGroupEntryDto>(defect));
    }
}
