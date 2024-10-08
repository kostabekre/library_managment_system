using LibraryManagementSystemAPI.Context;
using LibraryManagementSystemAPI.Publisher.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystemAPI.Publisher;

public class EfCorePublisherRepository : IPublisherRepository
{
    private readonly BookContext _bookContext;

    public EfCorePublisherRepository(BookContext bookContext)
    {
        _bookContext = bookContext;
    }

    public async Task<bool> IsPublisherUniqueAsync(string name) => await _bookContext
        .Publishers
        .AnyAsync(p => p.Name == name) == false;

    public async Task<bool> IsPublisherExistsAsync(int id)
    {
        var existingId =  await _bookContext.Publishers.Where(p => p.Id == id).Select(a => a.Id).FirstOrDefaultAsync();
        return existingId == id;
    }

    public async Task<PublisherFullInfo?> GetPublisherAsync(int id)
    {
        var publisher = await _bookContext.Publishers
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (publisher == null)
        {
            return null;
        }
        
        return (PublisherFullInfo?)publisher;
    }

    public async Task<PublisherFullInfo> CreatePublisherAsync(PublisherInfo info)
    {
        var publisher = (Data.Publisher)info;
        _bookContext.Publishers.Add(publisher);
        await _bookContext.SaveChangesAsync();
        return (PublisherFullInfo)publisher;
    }

    public async Task<bool> UpdatePublisherAsync(int id, PublisherInfo info)
    {
        var updatedRows = await _bookContext.Publishers
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(properties => properties
                .SetProperty(p => p.Address, info.Address)
                .SetProperty(p => p.Name, info.Name));
        
        return updatedRows > 0;
    }

    public async Task<bool> DeletePublisherAsync(int id)
    {
        var deletedRows = await _bookContext.Publishers
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
        return deletedRows > 0;
    }

    public async Task<IEnumerable<PublisherFullInfo>> GetAllPublishersAsync()
    {
        return await _bookContext.Publishers
            .AsNoTracking()
            .Select(p => (PublisherFullInfo)p)
            .ToListAsync();
    }
}