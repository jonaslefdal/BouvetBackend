using BouvetBackend.Entities;
using BouvetBackend.DataAccess;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BouvetBackend.Repositories
{
    public class EfTransportEntryRepository : ITransportEntryRepository
    {
        private readonly DataContext _context;

        public EfTransportEntryRepository(DataContext context)
        {
            _context = context;
        }

        public void Upsert(TransportEntry transportEntry)
        {
            // Retrieve the associated user
            var user = _context.Users.FirstOrDefault(u => u.UserId == transportEntry.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var existing = _context.TransportEntry
                .FirstOrDefault(a => a.TransportEntryId == transportEntry.TransportEntryId);

            if (existing != null)
            {
                int pointDifference = transportEntry.Points - existing.Points;

                existing.Method = transportEntry.Method;
                existing.Points = transportEntry.Points;
                existing.Co2 = transportEntry.Co2;
                existing.DistanceKm = transportEntry.DistanceKm;
                existing.CreatedAt = transportEntry.CreatedAt;
                
                user.TotalScore += pointDifference;
            }
            else
            {
                _context.TransportEntry.Add(transportEntry);
                user.TotalScore += transportEntry.Points;
            }

            _context.SaveChanges();
        }


        public double GetTotalCo2SavingsByUser(int userId)
        {
            return _context.TransportEntry
                .Where(te => te.UserId == userId)
                .Sum(te => te.Co2);
        }
        public double GetTotalMoneySaved(int userId)
        {
            return _context.TransportEntry
                .Where(te => te.UserId == userId)
                .Sum(te => te.MoneySaved);
        }

        public TransportEntry? Get(int TransportEntryId)
        {
            return _context.TransportEntry.FirstOrDefault(a => a.TransportEntryId == TransportEntryId);
        }

        public List<TransportEntry> GetAll()
        {
            return _context.TransportEntry.ToList();
        }

        public int GetTotalTravelCountByUser(int userId)
        {
            return _context.TransportEntry.Count(te => te.UserId == userId);
        }
        public int GetTransportEntryCount(int userId, Methode method, DateTime since)
        {
            return _context.TransportEntry.Count(te =>
                te.UserId == userId &&
                te.Method == method &&
                te.CreatedAt >= since);
        }
        public List<TransportEntry> GetEntriesForUser(int userId)
        {
            return _context.TransportEntry
                .AsNoTracking()
                .Where(te => te.UserId == userId)
                .ToList();
        }

        public int GetTransportDistanceCount(int userId, Methode method, DateTime since, double requiredDistanceKm)
        {
            return _context.TransportEntry.Count(te =>
                te.UserId == userId &&
                te.Method == method &&
                te.CreatedAt >= since &&
                te.DistanceKm >= requiredDistanceKm);
        }

        public double GetTransportDistanceSum(int userId, Methode method, DateTime since)
        {
            return _context.TransportEntry
                .Where(te => te.UserId == userId &&
                            te.Method == method &&
                            te.CreatedAt >= since)
                .Sum(te => te.DistanceKm);
        }



    }
}
