﻿using Microsoft.EntityFrameworkCore;
using TournamentPlanner.API.Data.Repositories;
using TournamentPlanner.API.Data.Sql.Models;
using TournamentPlanner.Data.IRepository;
using TournamentPlanner.DTOs;
using TournamentPlanner.Exceptions;
using TournamentPlanner.Services.Exceptions;
using TournamentPlanner.Services.Interface;

namespace TournamentPlanner.API.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<MatchDTO> CreateMatchAsync(MatchForCreateDto match)
        {
            var matchesCount = await _matchRepository.GetMatchesCountAsync();
            if(matchesCount > 32)
            {
                throw new MaxMatchesCountReachedException("Matches count cannot be more than 32");
            }
            var matchEntity = new Match()
            {
                RoundNumber = match.RoundNumber,
                Player1Id = match.Player1Id,
                Player2Id = match.Player2Id,
                WinnerId = match.WinnerId,
            };
            await _matchRepository.Create(matchEntity);

            MatchDTO companyToReturn = _matchRepository.FindByCondition(x => x.Id == matchEntity.Id).ToList()
                .Select(x => new MatchDTO(x.Id, x.RoundNumber)).FirstOrDefault();

            return companyToReturn;
        }

        public async Task DeleteMatchAsync(int id)
        {
            var match = _matchRepository.FindByCondition(x => x.Id == id).FirstOrDefault();
            if (match is not null)
            {
                await _matchRepository.Delete(match);

            }
            else
            {
                throw new MatchNotFoundIdRequestExeption("Match with given Id does not exists");
            }
        }

        public async Task GenerateMatchesForNextRound()
        {
            await _matchRepository.GenerateMatchesForNextRoundAsync();
        }

        public async Task<IEnumerable<MatchDTO>> GetAllMatchesAsync()
        {
            var matches = _matchRepository.GetAll();
            var matchesDTO = matches.Select(x => new MatchDTO(x.Id, x.RoundNumber)).ToList();
            return matchesDTO;
        }

        public async Task<IEnumerable<MatchDTO>> GetIncompleteMatches()
        {
            var matches = await _matchRepository.GetIncompleteMatchesAsync();
            var matchesDTO = matches.Select(x => new MatchDTO(x.Id, x.RoundNumber));
            return matchesDTO;
        }

        public async Task<MatchDTO> GetMatchByIdAsync(int id)
        {
            var match = _matchRepository.FindByCondition(x => x.Id == id).FirstOrDefault();
            if (match is not null)
            {
                var matchDTO = new MatchDTO(match.Id, match.RoundNumber);
                return matchDTO;
            }
            return null;
        }

        public async Task UpdateMatchAsync(int id, MatchForUpdateDTO match)
        {
            var currentMatch = _matchRepository.FindByCondition(x => x.Id == id).FirstOrDefault();
            if (currentMatch is not null)
            {
                _matchRepository.Update(currentMatch);
            }
            else
            {
                throw new MatchNotFoundIdRequestExeption("Match with given Id does not exists");
            }
        }
    }
}
