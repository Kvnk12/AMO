using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Service;
using Ninject;
using NTTData.AMOPortal.WebUI.Infrastructure;
using NTTData.AMOPortal.Data;



namespace NTTData.AMOPortal.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            

            //Repo binding
            ninjectKernel.Bind<IDbContextFactory>().To<DbContextFactory>();



            //Client
            ninjectKernel.Bind<IRepo<Client>>().To<Repo<Client>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Client>>().To<NTTData.AMOPortal.Service.CrudService<Client>>();
            ninjectKernel.Bind<Mapper.IMapper<Client, Dto.ClientInput>>().To<Mapper.Mapper<Client, Dto.ClientInput>>();
            
            //ninjectKernel.Bind<IRepo<MetricSpecification>>().To<Repo<MetricSpecification>>();
            //ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<MetricSpecification>>().To<NTTData.AMOPortal.Service.CrudService<MetricSpecification>>();
            //ninjectKernel.Bind<Mapper.IMapper<MetricSpecification, Dto.MetricSpecificationInput>>().To<Mapper.Mapper<MetricSpecification, Dto.MetricSpecificationInput>>();


            ninjectKernel.Bind<IRepo<Address>>().To<Repo<Address>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Address>>().To<NTTData.AMOPortal.Service.CrudService<Address>>();
            ninjectKernel.Bind<Mapper.IMapper<Address, Dto.Address>>().To<Mapper.Mapper<Address, Dto.Address>>();

            //Engagement
            ninjectKernel.Bind<IRepo<Engagements>>().To<Repo<Engagements>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Engagements>>().To<NTTData.AMOPortal.Service.CrudService<Engagements>>();
            ninjectKernel.Bind<Mapper.IMapper<Engagements, Dto.Engagements>>().To<Mapper.Mapper<Engagements, Dto.Engagements>>();


            //ninjectKernel.Bind<IRepo<Metric>>().To<Repo<Metric>>();
            //ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Metric>>().To<NTTData.AMOPortal.Service.CrudService<Metric>>();
            //ninjectKernel.Bind<Mapper.IMapper<Metric, Dto.MetricInput>>().To<Mapper.Mapper<Metric, Dto.MetricInput>>();

            //MetricSpecification
            ninjectKernel.Bind<IRepo<MetricSpecification>>().To<Repo<MetricSpecification>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<MetricSpecification>>().To<NTTData.AMOPortal.Service.CrudService<MetricSpecification>>();
            ninjectKernel.Bind<Mapper.IMapper<MetricSpecification, Dto.MetricSpecificationInput>>().To<Mapper.Mapper<MetricSpecification, Dto.MetricSpecificationInput>>();


            //MetricActual
            ninjectKernel.Bind<IRepo<MetricActual>>().To<Repo<MetricActual>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<MetricActual>>().To<NTTData.AMOPortal.Service.CrudService<MetricActual>>();
            ninjectKernel.Bind<Mapper.IMapper<MetricActual, Dto.MetricActualInput>>().To<Mapper.Mapper<MetricActual, Dto.MetricActualInput>>();

            //Incident
            ninjectKernel.Bind<IRepo<Incident>>().To<Repo<Incident>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Incident>>().To<NTTData.AMOPortal.Service.CrudService<Incident>>();
            ninjectKernel.Bind<Mapper.IMapper<Incident, Dto.IncidentInput>>().To<Mapper.Mapper<Incident, Dto.IncidentInput>>();


            //MaintenanceRequest

            ninjectKernel.Bind<IRepo<MaintenanceRequest>>().To<Repo<MaintenanceRequest>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<MaintenanceRequest>>().To<NTTData.AMOPortal.Service.CrudService<MaintenanceRequest>>();
            ninjectKernel.Bind<Mapper.IMapper<MaintenanceRequest, Dto.MaintenanceRequestInput>>().To<Mapper.Mapper<MaintenanceRequest, Dto.MaintenanceRequestInput>>();


            //Problem

            ninjectKernel.Bind<IRepo<Problem>>().To<Repo<Problem>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Problem>>().To<NTTData.AMOPortal.Service.CrudService<Problem>>();
            ////ninjectKernel.Bind<Mapper.IMapper<Problem, Dto.ProblemInput>>().To<Mapper.Mapper<Problem, Dto.ProblemInput>>();



            
            //Auth provider
           // ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
          
            //12/30 - Senthil - SLA Metric
            ninjectKernel.Bind<IRepo<Metric>>().To<Repo<Metric>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Metric>>().To<NTTData.AMOPortal.Service.CrudService<Metric>>();
            ninjectKernel.Bind<Mapper.IMapper<Metric, Dto.SLAMetricInput>>().To<Mapper.Mapper<Metric, Dto.SLAMetricInput>>();

            ninjectKernel.Bind<IRepo<ChartIndicator>>().To<Repo<ChartIndicator>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ChartIndicator>>().To<NTTData.AMOPortal.Service.CrudService<ChartIndicator>>();
            ninjectKernel.Bind<Mapper.IMapper<ChartIndicator, Dto.SLAChartIndInput>>().To<Mapper.Mapper<ChartIndicator, Dto.SLAChartIndInput>>();

            ninjectKernel.Bind<IRepo<ObjectiveType>>().To<Repo<ObjectiveType>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ObjectiveType>>().To<NTTData.AMOPortal.Service.CrudService<ObjectiveType>>();
            ninjectKernel.Bind<Mapper.IMapper<ObjectiveType, Dto.SLAObjTypeInput>>().To<Mapper.Mapper<ObjectiveType, Dto.SLAObjTypeInput>>();

            ninjectKernel.Bind<IRepo<Calculation>>().To<Repo<Calculation>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Calculation>>().To<NTTData.AMOPortal.Service.CrudService<Calculation>>();
            ninjectKernel.Bind<Mapper.IMapper<Calculation, Dto.SLACalcInput>>().To<Mapper.Mapper<Calculation, Dto.SLACalcInput>>();

            ninjectKernel.Bind<IRepo<ChartIndicator>>().To<Repo<ChartIndicator>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ChartIndicator>>().To<NTTData.AMOPortal.Service.CrudService<ChartIndicator>>();
            ninjectKernel.Bind<Mapper.IMapper<ChartIndicator, Dto.SLAChartIndInput>>().To<Mapper.Mapper<ChartIndicator, Dto.SLAChartIndInput>>();

            ninjectKernel.Bind<IRepo<CalculationDataElement>>().To<Repo<CalculationDataElement>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<CalculationDataElement>>().To<NTTData.AMOPortal.Service.CrudService<CalculationDataElement>>();
            ninjectKernel.Bind<Mapper.IMapper<CalculationDataElement, Dto.SLACalcDataInput>>().To<Mapper.Mapper<CalculationDataElement, Dto.SLACalcDataInput>>();

            ninjectKernel.Bind<IRepo<PrimaryAudience>>().To<Repo<PrimaryAudience>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<PrimaryAudience>>().To<NTTData.AMOPortal.Service.CrudService<PrimaryAudience>>();
            ninjectKernel.Bind<Mapper.IMapper<PrimaryAudience, Dto.SLAPYAudInput>>().To<Mapper.Mapper<PrimaryAudience, Dto.SLAPYAudInput>>();
            
            //12/30 - Senthil - SLA Metric


            ///10th Jan Ratna Kumar


            ninjectKernel.Bind<IRepo<ClientAccountContract>>().To<Repo<ClientAccountContract>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ClientAccountContract>>().To<NTTData.AMOPortal.Service.CrudService<ClientAccountContract>>();
            ninjectKernel.Bind<Mapper.IMapper<ClientAccountContract, Dto.ClientAccountContractInput>>().To<Mapper.Mapper<ClientAccountContract, Dto.ClientAccountContractInput>>();


            ninjectKernel.Bind<IRepo<ClientAndAccounts>>().To<Repo<ClientAndAccounts>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ClientAndAccounts>>().To<NTTData.AMOPortal.Service.CrudService<ClientAndAccounts>>();
            ninjectKernel.Bind<Mapper.IMapper<ClientAndAccounts, Dto.ClientAndAccountsInput>>().To<Mapper.Mapper<ClientAndAccounts, Dto.ClientAndAccountsInput>>();



            ninjectKernel.Bind<IRepo<VariableTypesAndMetaData>>().To<Repo<VariableTypesAndMetaData>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<VariableTypesAndMetaData>>().To<NTTData.AMOPortal.Service.CrudService<VariableTypesAndMetaData>>();
       //     ninjectKernel.Bind<Mapper.IMapper<VariableTypesAndMetaData, Dto.VariableTypesInput>>().To<Mapper.Mapper<VariableTypesAndMetaData, Dto.VariableTypesInput>>();



            ninjectKernel.Bind<IRepo<RegDataSet>>().To<Repo<RegDataSet>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<RegDataSet>>().To<NTTData.AMOPortal.Service.CrudService<RegDataSet>>();
     //       ninjectKernel.Bind<Mapper.IMapper<RegDataSet,  Dto.DataSetupInput>>().To<Mapper.Mapper<RegDataSet, Dto.DataSetupInput>>();



            ninjectKernel.Bind<IRepo<IMPostIdea>>().To<Repo<IMPostIdea>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<IMPostIdea>>().To<NTTData.AMOPortal.Service.CrudService<IMPostIdea>>();
            ninjectKernel.Bind<Mapper.IMapper<IMPostIdea, Dto.IMPostIdeaInput>>().To<Mapper.Mapper<IMPostIdea, Dto.IMPostIdeaInput>>();



            ninjectKernel.Bind<IRepo<IMPostChallenge>>().To<Repo<IMPostChallenge>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<IMPostChallenge>>().To<NTTData.AMOPortal.Service.CrudService<IMPostChallenge>>();
          //  ninjectKernel.Bind<Mapper.IMapper<IMPostChallenge, Dto.IMPostChallengeInput>>().To<Mapper.Mapper<IMPostChallenge, Dto.IMPostChallengeInput>>();


            //ninjectKernel.Bind<IRepo<IMChallengeIdeasRelations>>().To<Repo<IMChallengeIdeasRelations>>();
            //ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<IMChallengeIdeasRelations>>().To<NTTData.AMOPortal.Service.CrudService<IMChallengeIdeasRelations>>();
            //ninjectKernel.Bind<Mapper.IMapper<IMChallengeIdeasRelations, Dto.IMChallengeIdeasRelationsInput>>().To<Mapper.Mapper<IMChallengeIdeasRelations, Dto.IMChallengeIdeasRelationsInput>>();


            //ninjectKernel.Bind<IRepo<IMPostIdea>>().To<Repo<IMPostIdea>>();
            //ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<IMPostIdea>>().To<NTTData.AMOPortal.Service.CrudService<IMPostIdea>>();
            //ninjectKernel.Bind<Mapper.IMapper<IMPostIdea, Dto.IMPostIdeaInput>>().To<Mapper.Mapper<IMPostIdea, Dto.IMPostIdeaInput>>();

            ninjectKernel.Bind<IRepo<IMItemITBenefits>>().To<Repo<IMItemITBenefits>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<IMItemITBenefits>>().To<NTTData.AMOPortal.Service.CrudService<IMItemITBenefits>>();
            ninjectKernel.Bind<Mapper.IMapper<IMItemITBenefits, Dto.IMItemITBenefitsInput>>().To<Mapper.Mapper<IMItemITBenefits, Dto.IMItemITBenefitsInput>>();


            ninjectKernel.Bind<IRepo<IMItemBusinessBenefits>>().To<Repo<IMItemBusinessBenefits>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<IMItemBusinessBenefits>>().To<NTTData.AMOPortal.Service.CrudService<IMItemBusinessBenefits>>();
            ninjectKernel.Bind<Mapper.IMapper<IMItemBusinessBenefits, Dto.IMItemBusinessBenefitsInput>>().To<Mapper.Mapper<IMItemBusinessBenefits, Dto.IMItemBusinessBenefitsInput>>();


            ninjectKernel.Bind<IRepo<IMItemComments>>().To<Repo<IMItemComments>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<IMItemComments>>().To<NTTData.AMOPortal.Service.CrudService<IMItemComments>>();
            ninjectKernel.Bind<Mapper.IMapper<IMItemComments, Dto.IMItemCommentsInput>>().To<Mapper.Mapper<IMItemComments, Dto.IMItemCommentsInput>>();



            ninjectKernel.Bind<IRepo<HeatMapBuilderSetup>>().To<Repo<HeatMapBuilderSetup>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<HeatMapBuilderSetup>>().To<NTTData.AMOPortal.Service.CrudService<HeatMapBuilderSetup>>();
          //  ninjectKernel.Bind<Mapper.IMapper<HeatMapBuilderSetup, Dto.HeatMapBuilderSetupInput>>().To<Mapper.Mapper<HeatMapBuilderSetup, Dto.HeatMapBuilderSetupInput>>();




            /// From Senthil -Anadhi
            /// 
            ninjectKernel.Bind<IRepo<ClientAccounSetup>>().To<Repo<ClientAccounSetup>>(); 
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ClientAccounSetup>>().To<NTTData.AMOPortal.Service.CrudService<ClientAccounSetup>>();
            ninjectKernel.Bind<Mapper.IMapper<ClientAccounSetup, Dto.ClientAccountInput>>().To<Mapper.Mapper<ClientAccounSetup, Dto.ClientAccountInput>>();

            //From Raj -BAM

            ninjectKernel.Bind<IRepo<Verticals>>().To<Repo<Verticals>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Verticals>>().To<NTTData.AMOPortal.Service.CrudService<Verticals>>();
            ninjectKernel.Bind<Mapper.IMapper<Verticals, Dto.VerticalsInput>>().To<Mapper.Mapper<Verticals, Dto.VerticalsInput>>();

            ninjectKernel.Bind<IRepo<SubVerticals>>().To<Repo<SubVerticals>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<SubVerticals>>().To<NTTData.AMOPortal.Service.CrudService<SubVerticals>>();
            ninjectKernel.Bind<Mapper.IMapper<SubVerticals, Dto.SubVerticalsInput>>().To<Mapper.Mapper<SubVerticals, Dto.SubVerticalsInput>>();


            ninjectKernel.Bind<IRepo<VerticalAndSubVertical>>().To<Repo<VerticalAndSubVertical>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<VerticalAndSubVertical>>().To<NTTData.AMOPortal.Service.CrudService<VerticalAndSubVertical>>();
            ninjectKernel.Bind<Mapper.IMapper<VerticalAndSubVertical, Dto.VerticalAndSubVerticalInput>>().To<Mapper.Mapper<VerticalAndSubVertical, Dto.VerticalAndSubVerticalInput>>();




            ////ninjectKernel.Bind<IRepo<BusinessCapabilitiesLevel1>>().To<Repo<BusinessCapabilitiesLevel1>>();
            ////ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<BusinessCapabilitiesLevel1>>().To<NTTData.AMOPortal.Service.CrudService<BusinessCapabilitiesLevel1>>();
            ////ninjectKernel.Bind<Mapper.IMapper<BusinessCapabilitiesLevel1, Dto.BusinessCapabilitiesLevel1Input>>().To<Mapper.Mapper<BusinessCapabilitiesLevel1, Dto.BusinessCapabilitiesLevel1Input>>();


            ////ninjectKernel.Bind<IRepo<BusinessCapabilitiesLevel2>>().To<Repo<BusinessCapabilitiesLevel2>>();
            ////ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<BusinessCapabilitiesLevel2>>().To<NTTData.AMOPortal.Service.CrudService<BusinessCapabilitiesLevel2>>();
            ////ninjectKernel.Bind<Mapper.IMapper<BusinessCapabilitiesLevel2, Dto.BusinessCapabilitiesLevel2Input>>().To<Mapper.Mapper<BusinessCapabilitiesLevel2, Dto.BusinessCapabilitiesLevel2Input>>();


          
            ////ninjectKernel.Bind<IRepo<BusinessCapabilitiesLevel3>>().To<Repo<BusinessCapabilitiesLevel3>>();
            ////ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<BusinessCapabilitiesLevel3>>().To<NTTData.AMOPortal.Service.CrudService<BusinessCapabilitiesLevel3>>();
            ////ninjectKernel.Bind<Mapper.IMapper<BusinessCapabilitiesLevel3, Dto.BusinessCapabilitiesLevel3Input>>().To<Mapper.Mapper<BusinessCapabilitiesLevel3, Dto.BusinessCapabilitiesLevel3Input>>();

            ////ninjectKernel.Bind<IRepo<BusinessCapabilitiesLevels>>().To<Repo<BusinessCapabilitiesLevels>>();
            ////ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<BusinessCapabilitiesLevels>>().To<NTTData.AMOPortal.Service.CrudService<BusinessCapabilitiesLevels>>();
            ////ninjectKernel.Bind<Mapper.IMapper<BusinessCapabilitiesLevels, Dto.BusinessCapabilitiesLevelsInput>>().To<Mapper.Mapper<BusinessCapabilitiesLevels, Dto.BusinessCapabilitiesLevelsInput>>();


            //Added by Ratna On 21st Feb 2014


            ninjectKernel.Bind<IRepo<mdmVariable>>().To<Repo<mdmVariable>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<mdmVariable>>().To<NTTData.AMOPortal.Service.CrudService<mdmVariable>>();
        //    ninjectKernel.Bind<Mapper.IMapper<mdmVariable, Dto.VariableInput>>().To<Mapper.Mapper<mdmVariable, Dto.VariableInput>>();



            ninjectKernel.Bind<IRepo<mdmVariableLabel>>().To<Repo<mdmVariableLabel>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<mdmVariableLabel>>().To<NTTData.AMOPortal.Service.CrudService<mdmVariableLabel>>();
        //    ninjectKernel.Bind<Mapper.IMapper<mdmVariableLabel, Dto.mdmVariableLabelInput>>().To<Mapper.Mapper<mdmVariableLabel, Dto.mdmVariableLabelInput>>();


            ninjectKernel.Bind<IRepo<mdmVariableMD>>().To<Repo<mdmVariableMD>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<mdmVariableMD>>().To<NTTData.AMOPortal.Service.CrudService<mdmVariableMD>>();
        //    ninjectKernel.Bind<Mapper.IMapper<mdmVariableMD, Dto.mdmVariableMdInput>>().To<Mapper.Mapper<mdmVariableMD, Dto.mdmVariableMdInput>>();

            ninjectKernel.Bind<IRepo<mdmVariableREL>>().To<Repo<mdmVariableREL>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<mdmVariableREL>>().To<NTTData.AMOPortal.Service.CrudService<mdmVariableREL>>();
        //    ninjectKernel.Bind<Mapper.IMapper<mdmVariableREL, Dto.mdmVariableRelInput>>().To<Mapper.Mapper<mdmVariableREL, Dto.mdmVariableRelInput>>();

            //Added by Raj on 21st Feb 2014

            ninjectKernel.Bind<IRepo<LineOfBusinesses>>().To<Repo<LineOfBusinesses>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<LineOfBusinesses>>().To<NTTData.AMOPortal.Service.CrudService<LineOfBusinesses>>();
            ninjectKernel.Bind<Mapper.IMapper<LineOfBusinesses, Dto.LineOfBusinessInput>>().To<Mapper.Mapper<LineOfBusinesses, Dto.LineOfBusinessInput>>();

            ninjectKernel.Bind<IRepo<ApplicationLOBJunctions>>().To<Repo<ApplicationLOBJunctions>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ApplicationLOBJunctions>>().To<NTTData.AMOPortal.Service.CrudService<ApplicationLOBJunctions>>();
            ninjectKernel.Bind<Mapper.IMapper<ApplicationLOBJunctions, Dto.ApplicationLOBJunctionInput>>().To<Mapper.Mapper<ApplicationLOBJunctions, Dto.ApplicationLOBJunctionInput>>();


            ninjectKernel.Bind<IRepo<LOBandApplications>>().To<Repo<LOBandApplications>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<LOBandApplications>>().To<NTTData.AMOPortal.Service.CrudService<LOBandApplications>>();
            ninjectKernel.Bind<Mapper.IMapper<LOBandApplications, Dto.LOBandApplicationsInput>>().To<Mapper.Mapper<LOBandApplications, Dto.LOBandApplicationsInput>>();

            //Added by Raj on 3rd march 2014

            ninjectKernel.Bind<IRepo<ClientBCLevel1>>().To<Repo<ClientBCLevel1>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ClientBCLevel1>>().To<NTTData.AMOPortal.Service.CrudService<ClientBCLevel1>>();
            ninjectKernel.Bind<Mapper.IMapper<ClientBCLevel1, Dto.ClientBCLevel1Input>>().To<Mapper.Mapper<ClientBCLevel1, Dto.ClientBCLevel1Input>>();


            ninjectKernel.Bind<IRepo<ClientBCLevel2>>().To<Repo<ClientBCLevel2>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ClientBCLevel2>>().To<NTTData.AMOPortal.Service.CrudService<ClientBCLevel2>>();
            ninjectKernel.Bind<Mapper.IMapper<ClientBCLevel2, Dto.ClientBCLevel2Input>>().To<Mapper.Mapper<ClientBCLevel2, Dto.ClientBCLevel2Input>>();

            ninjectKernel.Bind<IRepo<ClientBCLevel3>>().To<Repo<ClientBCLevel3>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ClientBCLevel3>>().To<NTTData.AMOPortal.Service.CrudService<ClientBCLevel3>>();
            ninjectKernel.Bind<Mapper.IMapper<ClientBCLevel3, Dto.ClientBCLevel3Input>>().To<Mapper.Mapper<ClientBCLevel3, Dto.ClientBCLevel3Input>>();

            ninjectKernel.Bind<IRepo<ClientBCLevels>>().To<Repo<ClientBCLevels>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ClientBCLevels>>().To<NTTData.AMOPortal.Service.CrudService<ClientBCLevels>>();
            ninjectKernel.Bind<Mapper.IMapper<ClientBCLevels, Dto.ClientBCLevelsInput>>().To<Mapper.Mapper<ClientBCLevels, Dto.ClientBCLevelsInput>>();

            ninjectKernel.Bind<IRepo<JunctionLOBClientBCs>>().To<Repo<JunctionLOBClientBCs>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<JunctionLOBClientBCs>>().To<NTTData.AMOPortal.Service.CrudService<JunctionLOBClientBCs>>();
            ninjectKernel.Bind<Mapper.IMapper<JunctionLOBClientBCs, Dto.JunctionLOBClientBCInput>>().To<Mapper.Mapper<JunctionLOBClientBCs, Dto.JunctionLOBClientBCInput>>();

            ninjectKernel.Bind<IRepo<JunctionAppLOBClientBCs>>().To<Repo<JunctionAppLOBClientBCs>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<JunctionAppLOBClientBCs>>().To<NTTData.AMOPortal.Service.CrudService<JunctionAppLOBClientBCs>>();
            ninjectKernel.Bind<Mapper.IMapper<JunctionAppLOBClientBCs, Dto.JunctionAppLOBClientBCInput>>().To<Mapper.Mapper<JunctionAppLOBClientBCs, Dto.JunctionAppLOBClientBCInput>>();

            ninjectKernel.Bind<IRepo<ServiceReqeustDataSet>>().To<Repo<ServiceReqeustDataSet>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ServiceReqeustDataSet>>().To<NTTData.AMOPortal.Service.CrudService<ServiceReqeustDataSet>>();
            ninjectKernel.Bind<Mapper.IMapper<ServiceReqeustDataSet, Dto.SLAMDashboardInput>>().To<Mapper.Mapper<ServiceReqeustDataSet, Dto.SLAMDashboardInput>>();

            ninjectKernel.Bind<IRepo<SRFilters>>().To<Repo<SRFilters>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<SRFilters>>().To<NTTData.AMOPortal.Service.CrudService<SRFilters>>();
            ninjectKernel.Bind<Mapper.IMapper<SRFilters, Dto.SRFiltersInput>>().To<Mapper.Mapper<SRFilters, Dto.SRFiltersInput>>();

            ninjectKernel.Bind<IRepo<IdeaDataSet>>().To<Repo<IdeaDataSet>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<IdeaDataSet>>().To<NTTData.AMOPortal.Service.CrudService<IdeaDataSet>>();
       //     ninjectKernel.Bind<Mapper.IMapper<IdeaDataSet, Dto.IdeaInput>>().To<Mapper.Mapper<IdeaDataSet, Dto.IdeaInput>>();

            //Added by Raj on 5/19/2014

            ninjectKernel.Bind<IRepo<ServiceCalendar>>().To<Repo<ServiceCalendar>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ServiceCalendar>>().To<NTTData.AMOPortal.Service.CrudService<ServiceCalendar>>();
            ninjectKernel.Bind<Mapper.IMapper<ServiceCalendar, Dto.ServiceCalendarInput>>().To<Mapper.Mapper<ServiceCalendar, Dto.ServiceCalendarInput>>();


            ninjectKernel.Bind<IRepo<ServiceYear>>().To<Repo<ServiceYear>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ServiceYear>>().To<NTTData.AMOPortal.Service.CrudService<ServiceYear>>();
            ninjectKernel.Bind<Mapper.IMapper<ServiceYear, Dto.ServiceYearInput>>().To<Mapper.Mapper<ServiceYear, Dto.ServiceYearInput>>();


            ninjectKernel.Bind<IRepo<ClassAndCategory>>().To<Repo<ClassAndCategory>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<ClassAndCategory>>().To<NTTData.AMOPortal.Service.CrudService<ClassAndCategory>>();
            ninjectKernel.Bind<Mapper.IMapper<ClassAndCategory, Dto.ClassAndCategoryInput>>().To<Mapper.Mapper<ClassAndCategory, Dto.ClassAndCategoryInput>>();


            //Added by Raj on 10/11/2017 (below cant be added )

           // ninjectKernel.Bind<IRepo<webpages_Membership>>().To<Repo<webpages_Membership>>();
          //  ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<webpages_Membership>>().To<NTTData.AMOPortal.Service.CrudService<webpages_Membership>>();
            ninjectKernel.Bind<Mapper.IMapper<webpages_Membership, Dto.webpages_Membership>>().To<Mapper.Mapper<webpages_Membership, Dto.webpages_Membership>>();

            ninjectKernel.Bind<IRepo<UserPasswordLog>>().To<Repo<UserPasswordLog>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<UserPasswordLog>>().To<NTTData.AMOPortal.Service.CrudService<UserPasswordLog>>();
            ninjectKernel.Bind<Mapper.IMapper<UserPasswordLog, Dto.UserPasswordLogModel>>().To<Mapper.Mapper<UserPasswordLog, Dto.UserPasswordLogModel>>();

            ninjectKernel.Bind<IRepo<Role>>().To<Repo<Role>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<Role>>().To<NTTData.AMOPortal.Service.CrudService<Role>>();

            ninjectKernel.Bind<IRepo<mdmDataUploadTable>>().To<Repo<mdmDataUploadTable>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<mdmDataUploadTable>>().To<NTTData.AMOPortal.Service.CrudService<mdmDataUploadTable>>();
         //   ninjectKernel.Bind<Mapper.IMapper<mdmDataUploadTable, Dto.mdmDataUploadTableInput>>().To<Mapper.Mapper<mdmDataUploadTable, Dto.mdmDataUploadTableInput>>();

            ninjectKernel.Bind<IRepo<mdmhstDataFileupload>>().To<Repo<mdmhstDataFileupload>>();
            ninjectKernel.Bind<NTTData.AMOPortal.Domain.Service.ICrudService<mdmhstDataFileupload>>().To<NTTData.AMOPortal.Service.CrudService<mdmhstDataFileupload>>();
        //    ninjectKernel.Bind<Mapper.IMapper<mdmhstDataFileupload, Dto.mdmhstDataFileuploadInput>>().To<Mapper.Mapper<mdmhstDataFileupload, Dto.mdmhstDataFileuploadInput>>();



        }
    }
}